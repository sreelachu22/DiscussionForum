using DiscussionForum.Repositories;

namespace DiscussionForum.UnitOfWork
{
    public interface IUnitOfWork
    {
        IForumStatusRepository ForumStatus { get; }
        int Complete();
    }
}
