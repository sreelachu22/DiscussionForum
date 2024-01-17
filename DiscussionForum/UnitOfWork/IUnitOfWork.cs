using DiscussionForum.Repositories;
using DiscussionForum.Services;

namespace DiscussionForum.UnitOfWork
{
    public interface IUnitOfWork
    {       
        IForumCategoryRepository ForumCategory { get; }
        IForumStatusRepository ForumStatus { get; }
        int Complete();
    }
}
