using DiscussionForum.Models.APIModels;

namespace DiscussionForum.Services
{
    public interface IPointService
    {
        Task<string> UpdatePoint(int communityID, PointDto pointDto);
        Task<PointDto> GetPointsByCommunityId(int communityId);
        Task ThreadCreated(Guid createdBy);
        Task ThreadUpdated(Guid updatedBy);
        Task ThreadDeleted(Guid deletedBy);
        Task ThreadUpvoted(Guid upVotedBy, long threadId);
        Task ThreadDownvoted(Guid downVotedBy, long threadId);
        Task RemoveThreadUpvote(Guid upVotedBy, long threadId);
        Task RemoveThreadDownvote(Guid downVotedBy, long threadId);
        Task ReplyCreated(Guid createdBy);
        Task ReplyUpdated(Guid updatedBy);
        Task ReplyDeleted(Guid deletedBy);
        Task ReplyUpvoted(Guid upVotedBy, long replyId);
        Task ReplyDownvoted(Guid downVotedBy, long replyId);
        Task RemoveReplyUpvote(Guid upVotedBy, long replyId);
        Task RemoveReplyDownvote(Guid downVotedBy, long replyId);
    }
}
