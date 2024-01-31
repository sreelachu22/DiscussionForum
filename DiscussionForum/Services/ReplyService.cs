using DiscussionForum.Data;
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

        public async Task<IEnumerable<Reply>> GetRepliesFromDatabaseAsync()
        {
            try
            {
                /*var d = _context.Threads.ToList();*/
                return await _context.Replies.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetRepliesFromDatabaseAsync: {ex.Message}");
                throw;
            }
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
            Reply reply = new Reply{ ThreadID = _threadID, Content = _content, ParentReplyID = _parentReplyId, IsDeleted = false };
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

    }
}
