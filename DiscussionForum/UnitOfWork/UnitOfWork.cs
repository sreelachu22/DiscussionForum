using DiscussionForum.Data;
using DiscussionForum.Repositories;

namespace DiscussionForum.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            ForumStatus = new ForumStatusRepository(_context);
        }

        public IForumStatusRepository ForumStatus { get; }
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
