using DiscussionForum.Data;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.UnitOfWork;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Threading;

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
        public async Task<Reply> GetReplyByIdAsync(long replyID)
        {
            try
            {
                return await Task.FromResult(_context.Replies.Find(replyID));
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

        public async Task<IEnumerable<Reply>> GetRepliesByParentReplyIdAsync(long parentReplyID)
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
        }

        public async Task<Reply> CreateReplyAsync(long threadID, Guid creatorID, string content, long? parentReplyId)
        {
            try
            {
                Threads _thread = await Task.FromResult(_context.Threads.Find(threadID));
                User _creator = await Task.FromResult(_context.Users.Find(creatorID));
                //Checks if the thread is valid
                if (_thread == null)
                {
                    throw new Exception("Thread not found");
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
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while creating a reply.", ex);
            }
        }
        private Reply CreateReply(long threadID, Guid creatorID, string content, long? parentReplyId)
        {
            //Creates a new reply and saves it to the database
            try
            {
                Reply _reply = new Reply { ThreadID = threadID, Content = content, ParentReplyID = parentReplyId, IsDeleted = false, CreatedBy = creatorID, CreatedAt = DateTime.Now };

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
                else if (_reply != null && !_reply.IsDeleted)
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

        //Returns the all the replies of a post with the option to paginate
        //The nested replies of a reply is recursively added into the NestedReplies list of the ReplyDTO object
        //It take the threadId of the post to fetch replies. The replies will be starting from the parentReplyId provided
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
                        CreatedUserName = r.CreatedByUser != null ? r.CreatedByUser.Name : "",
                        CreatedBy = r.CreatedBy,
                        CreatedAt = r.CreatedAt,
                        ModifiedBy = r.ModifiedBy,
                        ModifiedAt = r.ModifiedAt,
                        HasViewed = r.HasViewed,
                        NestedReplies = GetNestedReplies(_context.Replies.ToList(), r.ReplyID, _context)
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
                // Filter the list of replies to find those with the specified replyId as ParentReplyID
                var nestedReplies = replies
                    .Where(r => r.ParentReplyID == replyId)
                    .Select(reply =>
                    {
                        // Explicitly load the ReplyVotes collection for each nested reply
                        context.Entry(reply).Collection(r => r.ReplyVotes).Load();

                        // Map each nested reply to a new ReplyDTO object
                        return new ReplyDTO
                        {
                            ReplyID = reply.ReplyID,
                            ThreadID = reply.ThreadID,
                            ParentReplyID = reply.ParentReplyID,
                            Content = reply.Content,
                            UpvoteCount = reply.ReplyVotes != null ? reply.ReplyVotes.Count(rv => !rv.IsDeleted && rv.IsUpVote) : 0,
                            DownvoteCount = reply.ReplyVotes != null ? reply.ReplyVotes.Count(rv => !rv.IsDeleted && !rv.IsUpVote) : 0,
                            IsDeleted = reply.IsDeleted,
                            CreatedUserName = reply.CreatedByUser != null ? reply.CreatedByUser.Name : "",
                            CreatedBy = reply.CreatedBy,
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










    }
}
