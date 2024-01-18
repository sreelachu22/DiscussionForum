using DiscussionForum.Repositories;
using DiscussionForum.Services;

namespace DiscussionForum.UnitOfWork
{
    public interface IUnitOfWork
    {   
        IDesignationRepository Designations { get; }
        IRoleRepository Role { get; }    
        IForumCategoryRepository ForumCategory { get; }
        IForumStatusRepository ForumStatus { get; }
        IThreadStatusRepository ThreadStatus { get; }
        
        int Complete();

    }

}
