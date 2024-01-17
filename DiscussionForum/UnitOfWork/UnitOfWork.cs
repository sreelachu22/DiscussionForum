using DiscussionForum.Data;
using DiscussionForum.Repositories;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Role = new RoleRepository(_context);
            // Initialize other repositories.
        }

        
        public IRoleRepository Role { get; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        int IUnitOfWork.Complete()
        {
            throw new NotImplementedException();
        }
    }
}
