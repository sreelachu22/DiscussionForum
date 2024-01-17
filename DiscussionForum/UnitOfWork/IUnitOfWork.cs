using DiscussionForum.Repositories;

namespace DiscussionForum.UnitOfWork
{
    //namespace ThreadStatusService.Infrastructure.UnitOfWork
    //{
        public interface IUnitOfWork
        {
            IThreadStatusRepository ThreadStatus { get; }
            int Complete();
        }
    //}
}
