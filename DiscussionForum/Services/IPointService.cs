namespace DiscussionForum.Services
{
    public interface IPointService
    {
        Task PostCreated(Guid postedBy);
        Task PostUpdated(Guid updatedBy);
        Task PostDeleted(Guid deletedBy);
        Task ReplyCreated(Guid createdBy);
        Task ReplyUpdated(Guid updatedBy);
        Task ReplyDeleted(Guid deletedBy);
        Task ReplyUpvoted(Guid upVotedBy, long replyId);
        Task ReplyDownvoted(Guid downVotedBy, long replyId);
    }
}
