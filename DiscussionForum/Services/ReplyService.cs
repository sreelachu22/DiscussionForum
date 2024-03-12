using DiscussionForum.Data;
using DiscussionForum.ExceptionFilter;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.UnitOfWork;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Services
{
    public class ReplyService : IReplyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        private readonly IPointService _pointService;
        public ReplyService(IUnitOfWork unitOfWork, AppDbContext context, IPointService pointService)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _pointService = pointService;
        }

        public async Task<IEnumerable<Reply>> GetAllRepliesAsync()
        {
            try
            {
                return await Task.FromResult(_unitOfWork.Reply.GetAll());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occurred while retrieving all replies.", ex);
            }
        }
        public async Task<IQueryable<ReplyDTO>> GetReplyByIdAsync(long replyID)
        {
            try
            {
                var _reply = _context.Replies
                    .Include(r => r.Threads)
                    .Include(r => r.ParentReply)
                    .Include(r => r.ReplyVotes)
                    .Include(r => r.CreatedByUser)
                    .Where(r => r.ReplyID == replyID);

                if (_reply == null)
                {
                    throw new Exception("Reply not found");
                }
                else
                {
                    return _reply
                        .Select(r => new ReplyDTO
                        {
                            ReplyID = r.ReplyID,
                            ThreadID = r.ThreadID,
                            ParentReplyID = r.ParentReplyID,
                            Content = r.Content,
                            IsDeleted = r.IsDeleted,
                            CreatedBy = r.CreatedBy,
                            CreatedUserName = r.CreatedByUser != null ? r.CreatedByUser.Name : "",
                            CreatedAt = r.CreatedAt,
                            ModifiedBy = r.ModifiedBy,
                            ModifiedAt = r.ModifiedAt,
                            HasViewed = r.HasViewed,
                            ThreadOwnerEmail = r.Threads.CreatedByUser.Email,

                        });
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while retrieving reply with ID {replyID}.", ex);
            }
        }
        public async Task<IEnumerable<Reply>> GetRepliesByThreadIdAsync(long threadID)
        {
            try
            {
                Threads _thread = await Task.FromResult(_context.Threads.Find(threadID));
                //Checks if the thread is valid
                if (_thread == null)
                {
                    throw new Exception("Thread not found");
                }
                return await (from reply in _context.Replies
                              where reply.ThreadID == _thread.ThreadID
                              select new Reply
                              {
                                  ReplyID = reply.ReplyID,
                                  ThreadID = reply.ThreadID,
                                  Content = reply.Content,
                                  ParentReplyID = reply.ParentReplyID,
                                  IsDeleted = reply.IsDeleted,
                                  CreatedBy = reply.CreatedBy,
                                  CreatedAt = reply.CreatedAt
                              }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while retrieving replies for thread ID {threadID}.", ex);
            }
        }

        /*public async Task<IEnumerable<Reply>> GetRepliesByParentReplyIdAsync(long parentReplyID)
        {
            try
            {
                Reply _parentReply = await Task.FromResult(_context.Replies.Find(parentReplyID));
                //Checks if the parent reply is valid
                if (_parentReply == null)
                {
                    throw new Exception("Parent reply not found");
                }
                return await (from reply in _context.Replies
                              where reply.ParentReplyID == _parentReply.ReplyID
                              select new Reply
                              {
                                  ReplyID = reply.ReplyID,
                                  ThreadID = reply.ThreadID,
                                  Content = reply.Content,
                                  ParentReplyID = reply.ParentReplyID,
                                  IsDeleted = reply.IsDeleted,
                                  CreatedBy = reply.CreatedBy,
                                  CreatedAt = reply.CreatedAt
                              }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while retrieving replies for parent reply ID {parentReplyID}.", ex);
            }
        }*/

        public async Task<IEnumerable<ReplyDTO>> GetRepliesByParentReplyIdAsync(long threadID, long? parentReplyID)
        {
            try
            {
                // Include related entities
                var replies = await _context.Replies
                    .Include(r => r.ReplyVotes)
                    .Include(r => r.CreatedByUser)
                    .Where(r => r.ParentReplyID == parentReplyID && r.ThreadID == threadID)
                    .ToListAsync();

                // Map entities to DTOs
                var replyDTOs = replies.Select(r => new ReplyDTO
                {
                    ReplyID = r.ReplyID,
                    ThreadID = r.ThreadID,
                    ParentReplyID = r.ParentReplyID,
                    Content = r.Content,
                    UpvoteCount = r.ReplyVotes != null ? r.ReplyVotes.Count(rv => !rv.IsDeleted && rv.IsUpVote) : 0,
                    DownvoteCount = r.ReplyVotes != null ? r.ReplyVotes.Count(rv => !rv.IsDeleted && !rv.IsUpVote) : 0,
                    IsDeleted = r.IsDeleted,
                    CreatedBy = r.CreatedBy,
                    CreatedUserName = r.CreatedByUser != null ? r.CreatedByUser.Name : "",
                    CreatedAt = r.CreatedAt,
                    ModifiedBy = r.ModifiedBy,
                    ModifiedAt = r.ModifiedAt,
                    HasViewed = r.HasViewed,
                    ChildReplyCount = _context.Replies.Count(cr => cr.ParentReplyID == r.ReplyID) // Count of child replies
                });

                return replyDTOs;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while retrieving replies for parent reply ID {parentReplyID} and thread ID {threadID}.", ex);
            }
        }



        public async Task<Reply> CreateReplyAsync(long threadID, Guid creatorID, string content, long? parentReplyId)
        {
            /*try
            {*/
            Threads _thread = await Task.FromResult(_context.Threads.Find(threadID));
            User _creator = await Task.FromResult(_context.Users.Find(creatorID));
            //Checks if the thread is valid
            if (_thread == null)
            {
                throw new Exception("Thread not found");
            }
            else if (_thread.ThreadStatusID == 1)
            {
                throw new CustomException(444, "Thread is closed");
            }
            //Checks if the creator is valid
            else if (_creator == null)
            {
                throw new Exception("Creator not found");
            }
            //Checks if the parent reply is valid
            else if (parentReplyId != null)
            {
                Reply _parentReply = await Task.FromResult(_context.Replies.Find(parentReplyId));
                if (_parentReply == null)
                {
                    throw new Exception("Parent reply not found");
                }
            }
            return await Task.FromResult(CreateReply(threadID, creatorID, content, parentReplyId));
            /*}
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while creating a reply.", ex);
            }*/
        }
        private Reply CreateReply(long threadID, Guid creatorID, string content, long? parentReplyId)
        {
            //Creates a new reply and saves it to the database
            try
            {
                Reply _reply = new Reply { ThreadID = threadID, Content = content, ParentReplyID = parentReplyId, IsDeleted = false, HasViewed = false, CreatedBy = creatorID, CreatedAt = DateTime.Now };

                _pointService.ReplyCreated(creatorID);

                _unitOfWork.Reply.Add(_reply);
                _unitOfWork.Complete();

                return _reply;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while creating a reply.", ex);
            }
        }
        public async Task<Reply> UpdateReplyAsync(long replyID, Guid modifierID, string content)
        {
            try
            {
                Reply _reply = await Task.FromResult(_context.Replies.Find(replyID));
                User _modifier = await Task.FromResult(_context.Users.Find(modifierID));
                //Checks if modifier is valid
                if (_modifier == null)
                {
                    throw new Exception("Modifier not found");
                }
                //Checks if reply is valid and not deleted
                else if (_reply != null)
                {
                    _reply.Content = content;
                    _reply.ModifiedBy = modifierID;
                    _reply.ModifiedAt = DateTime.Now;

                    _pointService.ReplyUpdated(modifierID);

                    _context.SaveChanges();

                    return _reply;
                }
                //Checks if the reply is valid but deleted
                else if (_reply != null && _reply.IsDeleted)
                {
                    throw new Exception("Reply has been deleted.");
                }
                else
                {
                    throw new Exception("Reply not found.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while updating a reply with ID {replyID}.", ex);
            }
        }
        public async Task<Reply> DeleteReplyAsync(long replyID, Guid modifierID)
        {
            try
            {
                Reply _reply = await Task.FromResult(_context.Replies.Find(replyID));
                User _modifier = await Task.FromResult(_context.Users.Find(modifierID));
                //Checks if modifier is valid
                if (_modifier == null)
                {
                    throw new Exception("Modifier not found");
                }
                //Checks if reply is valid and not deleted
                else if (_reply != null && !_reply.IsDeleted)
                {
                    _reply.IsDeleted = true;
                    _reply.ModifiedBy = modifierID;
                    _reply.ModifiedAt = DateTime.Now;

                    _pointService.ReplyDeleted(modifierID);

                    _context.SaveChanges();

                    return _reply;
                }
                //Checks if the reply is valid but deleted
                else if (_reply != null && _reply.IsDeleted)
                {
                    throw new Exception("Reply already deleted.");
                }
                else
                {
                    throw new Exception("Reply not found.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while deleting a reply with ID {replyID}.", ex);
            }
        }

        // Retrieves all replies of a post based on the provided threadId and optional parentReplyId.
        // Input:
        //   - threadId: The ID of the thread for which replies are to be retrieved.
        //   - parentReplyId (optional): The ID of the parent reply if retrieving nested replies.
        //   - page (optional): The page number of results to retrieve (default: 1).
        //   - pageSize (optional): The number of replies per page (default: 10).
        // Output: IQueryable<ReplyDTO> representing the replies of the specified post.
        // Functionality:
        //   - Executes the query with pagination and materializes the results into a list of Reply entities.
        //   - calculating upvote and downvote counts.
        //   - Retrieves nested replies for each reply using the GetNestedReplies method.        

        public IQueryable<ReplyDTO> GetAllRepliesOfAPost(long threadId, long? parentReplyId, int page = 1, int pageSize = 10)
        {
            try
            {
                // Ensure page and pageSize are valid
                page = Math.Max(1, page);
                pageSize = Math.Max(1, pageSize);

                var query = _context.Replies
                    .Include(r => r.Threads)
                    .Include(r => r.ParentReply)
                    .Include(r => r.ReplyVotes)
                    .Include(r => r.CreatedByUser)
                    .Where(r => r.ThreadID == threadId && r.ParentReplyID == parentReplyId);


                var replies = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                // Map the Reply entities to ReplyDTO objects
                var replyDTOs = replies
                    .Select(r => new ReplyDTO
                    {
                        ReplyID = r.ReplyID,
                        ThreadID = r.ThreadID,
                        ParentReplyID = r.ParentReplyID,
                        Content = r.Content,
                        UpvoteCount = r.ReplyVotes != null ? r.ReplyVotes.Count(rv => !rv.IsDeleted && rv.IsUpVote) : 0,
                        DownvoteCount = r.ReplyVotes != null ? r.ReplyVotes.Count(rv => !rv.IsDeleted && !rv.IsUpVote) : 0,
                        IsDeleted = r.IsDeleted,
                        CreatedBy = r.CreatedBy,
                        CreatedUserName = r.CreatedByUser != null ? r.CreatedByUser.Name : "",
                        CreatedAt = r.CreatedAt,
                        ModifiedBy = r.ModifiedBy,
                        ModifiedAt = r.ModifiedAt,
                        HasViewed = r.HasViewed,
                        NestedReplies = GetNestedReplies(_context.Replies.Include(x => x.CreatedByUser).ToList(), r.ReplyID, _context)
                    }); ;

                return replyDTOs.AsQueryable();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        // Recursive method to retrieve nested replies for a given replyId
        static List<ReplyDTO> GetNestedReplies(List<Reply> replies, long replyId, AppDbContext context)
        {
            try
            {
                var nestedReplies = replies
                    .Where(r => r.ParentReplyID == replyId)
                    .Select(reply =>
                    {
                        context.Entry(reply).Collection(r => r.ReplyVotes).Load();

                        return new ReplyDTO
                        {
                            ReplyID = reply.ReplyID,
                            ThreadID = reply.ThreadID,
                            ParentReplyID = reply.ParentReplyID,
                            Content = reply.Content,
                            UpvoteCount = reply.ReplyVotes != null ? reply.ReplyVotes.Count(rv => !rv.IsDeleted && rv.IsUpVote) : 0,
                            DownvoteCount = reply.ReplyVotes != null ? reply.ReplyVotes.Count(rv => !rv.IsDeleted && !rv.IsUpVote) : 0,
                            IsDeleted = reply.IsDeleted,
                            CreatedBy = reply.CreatedBy,
                            //replies.FirstOrDefault()?.CreatedByUser?.Name ?? "";
                            CreatedUserName = reply.CreatedByUser != null ? reply.CreatedByUser.Name : "",
                            CreatedAt = reply.CreatedAt,
                            ModifiedBy = reply.ModifiedBy,
                            ModifiedAt = reply.ModifiedAt,
                            HasViewed = reply.HasViewed,
                            // Recursively call GetNestedReplies to retrieve nested replies of the current reply
                            NestedReplies = GetNestedReplies(replies, reply.ReplyID, context)

                        };
                    })
                    .ToList();

                return nestedReplies;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting nested replies: {ex.Message}");
                throw;
            }
        }


        // Retrieves unviewed replies for a specified user, optionally filtered by category, sorted by creation date.
        // Input:
        //   - userId: The ID of the user for whom unviewed replies are to be retrieved.
        //   - categoryId (optional): The ID of the category to filter replies (default: null).
        //   - sortDirection: The direction to sort replies ('asc' or 'desc').
        //   - pageNumber: The page number of results to retrieve.
        //   - pageSize: The number of replies per page.
        // Output: Tuple containing a collection of unviewed reply DTOs and the total count of unviewed replies.
        // Functionality:       
        //   - Filters replies based on whether the user is the creator of the parent reply or thread.        
        //   - Sorts replies based on the specified direction ('asc' or 'desc') by creation date.        
        //   - Paginates the query to retrieve replies for the specified page.                

        public (IEnumerable<ReplyNotifyDTO> replies, int totalCount) GetUnviewedReplies(Guid userId, int? categoryId, string sortDirection, int pageNumber, int pageSize)
        {
            IQueryable<Reply> query = _context.Replies
                .Where(r => r.HasViewed == false &&
                            ((r.ParentReply != null && r.ParentReply.CreatedBy == userId) ||
                             (r.ParentReply == null && r.Threads.CreatedBy == userId)))
                .Where(r => !(r.ParentReply != null && r.ParentReply.CreatedBy == userId &&
                              r.CreatedBy == userId) &&
                            !(r.ParentReply == null && r.Threads.CreatedBy == userId &&
                              r.CreatedBy == userId));

            if (categoryId.HasValue && categoryId != 0)
            {
                query = query.Where(r => r.Threads.CommunityCategoryMapping.CommunityCategory.CommunityCategoryID == categoryId.Value);
            }

            if (sortDirection.ToLower() == "desc")
            {
                query = query.OrderByDescending(r => r.CreatedAt);
            }
            else
            {
                query = query.OrderBy(r => r.CreatedAt);
            }

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var paginatedQuery = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var repliesForUser = paginatedQuery
                .Select(r => new ReplyNotifyDTO
                {
                    ChildReplyID = r.ReplyID,
                    ChildReplyContent = r.Content,
                    ChildReplyCreatedAt = r.CreatedAt,
                    ChildReplyUserName = r.CreatedByUser.Name,
                    ParentReplyID = r.ParentReplyID,
                    ParentReplyUserName = r.ParentReply != null ? r.ParentReply.CreatedByUser.Name :
                        (r.Threads.CreatedBy == userId ? r.Threads.CreatedByUser.Name : null),
                    ParentReplyContent = r.ParentReply != null ? r.ParentReply.Content :
                        (r.Threads.CreatedBy == userId ? r.Threads.Content : null),
                    CategoryName = r.Threads.CommunityCategoryMapping.CommunityCategory.CommunityCategoryName,
                    CommunityName = r.Threads.CommunityCategoryMapping.Community.CommunityName,
                    ThreadContent = r.Threads.Content
                })
                .ToList();

            return (repliesForUser, totalCount);
        }

        // Updates the 'HasViewed' status of a reply to mark it as viewed.
        // Input: The ID of the reply to update.
        // Output: A boolean indicating whether the operation was successful.
        // Functionality:       
        //   - Checks if the reply exists; returns false if not found.
        //   - Sets the 'HasViewed' property of the reply to true.                

        public async Task<bool> UpdateHasViewed(long replyId)
        {
            var reply = await _context.Replies.FindAsync(replyId);
            if (reply == null)
            {
                return false;
            }

            reply.HasViewed = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAllHasViewedAsync(long[] replyIDs)
        {
            // Assuming userId is the primary key for the user entity associated with replies
            Reply[] replies = await _context.Replies
                .Where(reply => replyIDs.Contains(reply.ReplyID) && reply.HasViewed == false)
                .ToArrayAsync();

            if (replies == null)
            {
                return false;
            }
            else
            {
                foreach (var reply in replies)
                {
                    reply.HasViewed = true;
                }
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<long> GetBestAnswerIdAsync(long threadId)
        {
            Threads _thread = await _context.Threads.FindAsync(threadId);
            if (_thread == null)
            {
                throw new CustomException(446, "Thread not found");
            }

            BestAnswer _bestAnswer = await _context.BestAnswers.Where(ba => ba.ThreadID == threadId && ba.IsDeleted == false).FirstOrDefaultAsync();
            if (_bestAnswer == null)
            {
                return 0;
            }
            return _bestAnswer.ReplyID;
        }

        public async Task<BestAnswer> MarkReplyAsBestAnswerAsync(long replyId, Guid createdBy)
        {
            Reply _reply = await _context.Replies.FindAsync(replyId);
            Threads _thread = await _context.Threads.FindAsync(_reply.ThreadID);
            User _creator = await _context.Users.FindAsync(createdBy);

            if (_reply == null)
            {
                throw new CustomException(442, "Reply not found");
            }
            else if(_thread == null)
            {
                throw new CustomException(446, "Thread not found");
            }
            else if (_creator == null)
            {
                throw new CustomException(445, "User not found");
            }
            else if(_creator.UserID != _thread.CreatedBy)
            {
                throw new CustomException(441, "User not authorized to mark reply as best answer of this thread");
            }

            BestAnswer _bestAnswer = new BestAnswer
            {
                ThreadID = _reply.ThreadID,
                ReplyID = _reply.ReplyID,
                IsDeleted = false,
                CreatedBy = _creator.UserID,
                CreatedAt = DateTime.Now
            };

            await _context.BestAnswers.AddAsync(_bestAnswer);
            await _context.SaveChangesAsync();

            return _bestAnswer;
        }

    }
}
