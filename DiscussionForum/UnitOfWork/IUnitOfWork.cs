using DiscussionForum.Repositories;
using DiscussionForum.Services;

namespace DiscussionForum.UnitOfWork
{
    public interface IUnitOfWork
    {   
        IDesignationRepository Designations { get; }
        IRoleRepository Role { get; }
        ICommunityCategoryRepository CommunityCategory { get; }
        ICommunityStatusRepository CommunityStatus { get; }
        IThreadStatusRepository ThreadStatus { get; }
        
        int Complete();

    }

}
