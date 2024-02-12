namespace DiscussionForum.Services
{
    public interface IPointService
    {
        Task ThreadCreated(Guid createdBy);
        Task ThreadUpdated(Guid updatedBy);
        Task ThreadDeleted(Guid deletedBy);
        Task ThreadUpvoted(Guid upVotedBy, long threadId);
        Task ThreadDownvoted(Guid downVotedBy, long threadId);
        Task ReplyCreated(Guid createdBy);
        Task ReplyUpdated(Guid updatedBy);
        Task ReplyDeleted(Guid deletedBy);
        Task ReplyUpvoted(Guid upVotedBy, long replyId);
        Task ReplyDownvoted(Guid downVotedBy, long replyId);
    }
}
