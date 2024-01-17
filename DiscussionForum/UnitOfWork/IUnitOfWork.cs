using DiscussionForum.Repositories;
using DiscussionForum.Services;

namespace DiscussionForum.UnitOfWork
{
    public interface IUnitOfWork
    {   
        IRoleRepository Role { get; }    
        IForumCategoryRepository ForumCategory { get; }
        IForumStatusRepository ForumStatus { get; }

        int Complete();
    }
}