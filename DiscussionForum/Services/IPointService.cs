namespace DiscussionForum.Services
{
    public interface IPointService
    {
        Task PostCreated(Guid guid);
        Task PostUpdated(Guid guid);
        Task PostDeleted(Guid guid);
        Task ReplyCreated(Guid guid);
        Task ReplyUpdated(Guid guid);
        Task ReplyDeleted(Guid guid);
    }
}
