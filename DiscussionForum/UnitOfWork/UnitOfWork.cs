using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;

using DiscussionForum.Repositories;

namespace DiscussionForum.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
          _context = context?? throw new ArgumentNullException(nameof(context));

            ForumCategory = new ForumCategoryRepository(_context);
            ForumStatus = new ForumStatusRepository(_context);
            // Initialize other repositories.
        }
        public IForumStatusRepository ForumStatus { get; }
        public IForumCategoryRepository ForumCategory { get;}
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
