using DiscussionForum.Data;
using DiscussionForum.Repositories;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.UnitOfWork
{
    //namespace ThreadStatusService.Infrastructure.UnitOfWork
    //{
        public class UnitOfWork : IUnitOfWork
        {
          _context = context?? throw new ArgumentNullException(nameof(context));
            
            Role = new RoleRepository(_context);
            ForumCategory = new ForumCategoryRepository(_context);
            ForumStatus = new ForumStatusRepository(_context);
            ThreadStatus = new ThreadStatusRepository(_context);
            // Initialize other repositories.
        }
        
        public IRoleRepository Role { get; }
        public IForumStatusRepository ForumStatus { get; }
        public IForumCategoryRepository ForumCategory { get;}
        public IThreadStatusRepository ThreadStatus { get; }
  
        public int Complete()
        {
            return _context.SaveChanges();
        }
  
        int IUnitOfWork.Complete()
        {
            throw new NotImplementedException();
        }
}
