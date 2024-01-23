﻿using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Services
{
    public interface IReplyService
    {
        Task<IEnumerable<Reply>> GetAllRepliesAsync();
        Task<Reply> GetReplyByIdAsync(long _replyID);
        Task<IEnumerable<Reply>> GetRepliesByThreadIdAsync(long _threadID);
        Task<IEnumerable<Reply>> GetRepliesByParentReplyIdAsync(long _parentReplyID);
        Task<Reply> CreateReplyAsync(long _threadID, long _parentReplyId, string _content);
        Task<Reply> UpdateReplyAsync(long _replyID, string _content);
        Task DeleteReplyAsync(long _replyID);

    }
}
