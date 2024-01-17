using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Repositories;

namespace DiscussionForum.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context) { }
    }
}
