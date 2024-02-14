using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using DiscussionForum.Repositories;
using DiscussionForum.Models.APIModels;

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

        /// <summary>
        /// Retrieves all roles
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>

        public Task<IEnumerable<Role>> GetAllRoles()
        {
            try {
                var roles = _unitOfWork.Role.GetAll();

                // Filter out the "SuperAdmin" role
                roles = roles.Where(role => role.RoleID!= 1);
                return Task.FromResult(roles);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new ApplicationException("Error occurred while fetching Roles", ex);
            }
            
        }



        /// <summary>
        /// Retrive single role by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public Task<Role> GetRoleByID(int id)
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
