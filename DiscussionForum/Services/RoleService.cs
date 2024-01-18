using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using DiscussionForum.Repositories;

namespace DiscussionForum.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public RoleService(IUnitOfWork unitOfWork,AppDbContext context) { 
            _unitOfWork = unitOfWork;
            _context = context;
        }


        public Task<IEnumerable<Role>> GetAllRoles()
        {
            try {
                var roles = _unitOfWork.Role.GetAll();
                return Task.FromResult(roles);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new ApplicationException("Error occurred while fetching Roles", ex);
            }
            
        }

        public  Task<Role> GetRoleByID(int id)
        {
            try
            {
                return Task.FromResult(_unitOfWork.Role.GetById(id));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occurred while fetching Roles", ex);
            }
        }

        
    }
}
