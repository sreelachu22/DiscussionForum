using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Services
{
    public interface IRoleService
    {
        Task<Role> GetRoleByID(int id);
        Task<IEnumerable<Role>> GetAllRoles();

    }
}
