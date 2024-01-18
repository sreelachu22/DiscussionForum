using DiscussionForum.Data;
using DiscussionForum.Repositories;

namespace DiscussionForum.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IDesignationRepository Designations { get; }
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            // Initialize other repositories.
            Designations = new DesignationRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }

    }

}
