using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Services
{
    public interface IReplyService
    {
        /// <summary>
        /// Retrieves all replies.
        /// </summary>
        Task<IEnumerable<Reply>> GetAllRepliesAsync();
        /// <summary>
        /// Retrieves a reply based on the given reply ID.
        /// </summary>
        /// <param name="replyID">The ID to search for in replies.</param>
        Task<IQueryable<ReplyDTO>> GetReplyByIdAsync(long replyID);
        /// <summary>
        /// Retrieves a list of replies based on the given thread ID.
        /// </summary>
        /// <param name="threadID">The ID of the thread to search for in replies.</param>
        Task<IEnumerable<Reply>> GetRepliesByThreadIdAsync(long threadID);
        /// <summary>
        /// Retrieves a list of replies based on the given parent reply ID.
        /// </summary>
        /// <param name="parentReplyID">The ID of the parent reply to search for in replies.</param>
        /// 



        /*Task<IEnumerable<Reply>> GetRepliesByParentReplyIdAsync(long parentReplyID);*/

        Task<IEnumerable<ReplyDTO>> GetRepliesByParentReplyIdAsync(long threadID, long? parentReplyID);




        /// <summary>
        /// Creates a new reply with content from request body.
        /// </summary>
        /// <param name="threadID">The ID of the thread to which reply is posted.</param>
        /// <param name="parentReplyID">The ID of the reply to which reply is posted. May be null if not applicable.</param>
        /// <param name="creatorID">The ID of the user posting the reply.</param>
        /// <param name="content">The content of the reply.</param>
        Task<Reply> CreateReplyAsync(long threadID, Guid creatorID, string content, long? parentReplyID);
        /// <summary>
        /// Updates a reply with content from request body based on the given reply ID.
        /// </summary>
        /// <param name="replyID">The ID of the reply to be updated.</param>
        /// <param name="modifierID">The ID of the user editing the reply.</param>
        /// <param name="content">The edited content of the reply.</param>
        Task<Reply> UpdateReplyAsync(long replyID, Guid modifierID, string content);
        /// <summary>
        /// Deletes a reply based on the given reply ID.
        /// </summary>
        /// <param name="replyID">The ID of the reply to be deleted.</param>
        /// <param name="modifierID">The ID of the user deleting the reply.</param>
        Task<Reply> DeleteReplyAsync(long replyID, Guid modifierID);
        IQueryable<ReplyDTO> GetAllRepliesOfAPost(long threadId, long? parentReplyId, int page = 1, int pageSize = 10);
        (IEnumerable<ReplyNotifyDTO> replies, int totalCount) GetUnviewedReplies(Guid userId, int? categoryId, string sortDirection, int pageNumber, int pageSize);

        Task<bool> UpdateHasViewed(long replyId);

    }
}
