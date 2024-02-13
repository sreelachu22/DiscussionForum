using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Type;

namespace DiscussionForum.Services
{
    public interface IRoleService
    {
        Task<Role> GetRoleByID(int id);
        Task<IEnumerable<Role>> GetAllRoles();

        /// <summary>
        /// Retrieves all roles from the IdentityRoles table in the database and returns them as a list of DropdownDto objects.
        /// The role names are modified by adding spaces before capital letters in the name.
        /// </summary>
        /// <returns>A service response containing the list of roles as DropdownDto objects with modified names.</returns>
        /*Task<ServiceResponse<List<ListDto<string>>>> GetAllRolesAsync();*/

    }
}
