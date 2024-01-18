using DiscussionForum.Repositories;

namespace DiscussionForum.UnitOfWork
{
    public interface IUnitOfWork
    {
        IDesignationRepository Designations { get; }
        int Complete();

    }

}
