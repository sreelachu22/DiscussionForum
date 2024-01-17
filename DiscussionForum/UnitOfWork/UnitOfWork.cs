using DiscussionForum.Data;
using DiscussionForum.Models;
using DiscussionForum.Repositories;

namespace DiscussionForum.UnitOfWork
{
    //namespace ThreadStatusService.Infrastructure.UnitOfWork
    //{
        public class UnitOfWork : IUnitOfWork
        {
            private readonly AppDbContext _context;

            public UnitOfWork(AppDbContext context)
            {
                _context = context;
                ThreadStatus = new ThreadStatusRepository(_context);
            }

            public IThreadStatusRepository ThreadStatus { get; }

            public int Complete()
            {
                return _context.SaveChanges();
            }

            public void Dispose()
            {
                _context.Dispose();
            }

        }
    //}

}