using DiscussionForum.Data;

namespace DiscussionForum.Services
{
    public class UserService:IUserService
    {
        private readonly IUserService _userService;
        private readonly AppDbContext _context;
    }
}
