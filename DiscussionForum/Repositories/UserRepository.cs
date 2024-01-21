using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Repositories
{
    public class UserRepository:Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }
    }
}
