using DiscussionForum.Data;
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

        public ReplyService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IEnumerable<Reply>> GetAllRepliesAsync()
        {
            try
            {
                return await Task.FromResult(_unitOfWork.Reply.GetAll());
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                throw new ApplicationException("Error occurred while retrieving all replies.", ex);
            }
        }
        public async Task<Reply> GetReplyByIdAsync(long _replyID)
        {
            try
            {
                return await Task.FromResult(_context.Replies.Find(_replyID));
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                throw new ApplicationException($"Error occurred while retrieving reply with ID {_replyID}.", ex);
            }
        }
        public async Task<IEnumerable<Reply>> GetRepliesByThreadIdAsync(long _threadID)
        {
            try
            {
                var result = await (from reply in _context.Replies
                                    where reply.ThreadID == _threadID
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
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                throw new ApplicationException($"Error occurred while retrieving replies for thread ID {_threadID}.", ex);
            }
        }

        public async Task<IEnumerable<Reply>> GetRepliesByParentReplyIdAsync(long _parentReplyID)
        {
            try
            {
                var result = await (from reply in _context.Replies
                                    where reply.ParentReplyID == _parentReplyID
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
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                throw new ApplicationException($"Error occurred while retrieving replies for parent reply ID {_parentReplyID}.", ex);
            }
        }

        public async Task<Reply> CreateReplyAsync(long _threadID, long _parentReplyId, string _content)
        {
            try
            {
                return await Task.FromResult(CreateReply(_threadID, _parentReplyId, _content));
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                throw new ApplicationException($"Error occurred while creating a reply.", ex);
            }
        }
        private Reply CreateReply(long _threadID, long _parentReplyId, string _content)
        {
            Reply reply = new Reply { ThreadID = _threadID, Content = _content, ParentReplyID = _parentReplyId, IsDeleted = false };
            _unitOfWork.Reply.Add(reply);
            _unitOfWork.Complete();
            return reply;
        }
        public async Task<Reply> UpdateReplyAsync(long _replyID, string _content)
        {
            try
            {
                return await Task.FromResult(UpdateReply(_replyID, _content));
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                throw new ApplicationException($"Error occurred while updating a reply with ID {_replyID}.", ex);
            }
        }
        private Reply UpdateReply(long _replyID, string _content)
        {
            var reply = _context.Replies.Find(_replyID);

            if (reply != null)
            {
                reply.Content = _content;
                _context.SaveChanges();
            }
            return reply;
        }
        public async Task DeleteReplyAsync(long _replyID)
        {
            try
            {
                await Task.Run(() => DeleteReply(_replyID));
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                throw new ApplicationException($"Error occurred while deleting reply with ID {_replyID}.", ex);
            }
        }
        private void DeleteReply(long _replyID)
        {
            var reply = _context.Replies.Find(_replyID);

            if (reply != null)
            {
                reply.IsDeleted = true;
                _context.SaveChanges();
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
                    .Include(r => r.Thread)
                    .Include(r => r.ParentReply)
                    .Where(r => r.ThreadID == threadId && r.ParentReplyID == parentReplyId);

                // Perform pagination on the retrieved replies
                var replies = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                // Map the Reply entities to ReplyDTO objects
                var replyDTOs = replies.Select(r => new ReplyDTO
                {
                    ReplyID = r.ReplyID,
                    ThreadID = r.ThreadID,
                    ParentReplyID = r.ParentReplyID,
                    Content = r.Content,
                    NestedReplies = GetNestedReplies(_context.Replies.ToList(), r.ReplyID)
                });

                return replyDTOs.AsQueryable();
            }
            catch (Exception ex)
            {                
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        // Recursive method to retrieve nested replies for a given replyId
        static List<ReplyDTO> GetNestedReplies(List<Reply> replies, long replyId)
        {
            try
            {
                // Filter the list of replies to find those with the specified replyId as ParentReplyID
                var nestedReplies = replies
                    .Where(r => r.ParentReplyID == replyId)
                    .Select(reply => new ReplyDTO
                    {
                        // Map each nested reply to a new ReplyDTO object
                        ReplyID = reply.ReplyID,
                        ThreadID = reply.ThreadID,
                        ParentReplyID = reply.ParentReplyID,
                        Content = reply.Content,
                        // Recursively call GetNestedReplies to retrieve nested replies of the current reply
                        NestedReplies = GetNestedReplies(replies, reply.ReplyID)
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



        //fetch replies from database
        public async Task<IEnumerable<Reply>> GetRepliesFromDatabaseAsync()
        {
            try
            {
                return await _context.Replies.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetRepliesFromDatabaseAsync: {ex.Message}");
                throw;
            }
        }

    }
}
