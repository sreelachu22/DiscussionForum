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
            CommunityCategory = new CommunityCategoryRepository(_context);
            CommunityStatus = new CommunityStatusRepository(_context);
            ThreadStatus = new ThreadStatusRepository(_context);
            // Initialize other repositories.
            Designations = new DesignationRepository(_context);
            Notice = new NoticeRepository(_context);
            User= new UserRepository(_context);
        }

        public IDesignationRepository Designations { get; }
        public IRoleRepository Role { get; }
        public ICommunityStatusRepository CommunityStatus { get; }
        public ICommunityCategoryRepository CommunityCategory { get; }
        public IThreadStatusRepository ThreadStatus { get; }
        public INoticeRepository Notice { get; }
        public IUserRepository User { get; }

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
