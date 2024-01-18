using DiscussionForum.Data;
using DiscussionForum.Repositories;
using DiscussionForum.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

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

            Role = new RoleRepository(_context);
            ForumCategory = new ForumCategoryRepository(_context);
            ForumStatus = new ForumStatusRepository(_context);
            ThreadStatus = new ThreadStatusRepository(_context);
            // Initialize other repositories.
            Designations = new DesignationRepository(_context);
        }

        public IDesignationRepository Designations { get; }
        public IRoleRepository Role { get; }
        public IForumStatusRepository ForumStatus { get; }
        public IForumCategoryRepository ForumCategory { get; }
        public IThreadStatusRepository ThreadStatus { get; }

        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose(){}

        /*int IUnitOfWork.Complete()
        {
            throw new NotImplementedException();
        }*/
    }

}
