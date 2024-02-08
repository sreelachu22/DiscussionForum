using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using DiscussionForum.Repositories;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Type;

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


        /* GetAllRoles retrieves all roles from the database, excluding the "SuperAdmin" role (RoleID=1).
         * The method uses the Unit of Work pattern and returns a Task<IEnumerable<Role>>. In case of an exception,
         * it logs the error message and throws an ApplicationException.*/
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



        /* GetRoleByID retrieves a role from the database based on the provided id. The method utilizes 
         * the Unit of Work pattern to access the role by ID and returns a Task<Role>. In case of an exception, 
         * it throws an ApplicationException with an informative error message*/
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

        /// <summary>
        /// Retrieves all roles from the IdentityRoles table in the database and returns them as a list of DropdownDto objects.
        /// The role names are modified by adding spaces before capital letters in the name.
        /// </summary>
        /// <returns>A service response containing the list of roles as DropdownDto objects with modified names.</returns>
        /*public async Task<ServiceResponse<List<ListDto<string>>>> GetAllRolesAsync()
        {
            var response = new ServiceResponse<List<ListDto<string>>>();
            response.Result = await _context.Roles
                .Select(r => new ListDto<string>
                {
                    Id = r.RoleId,
                    Name = AddSpacesToRoleName(r.Name)
                })
                .ToListAsync();
            return response;
        }*/


    }
}
