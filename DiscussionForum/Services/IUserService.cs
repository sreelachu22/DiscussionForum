using DiscussionForum.Models.APIModels;

namespace DiscussionForum.Services
{
    using DiscussionForum.Models.EntityModels;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<PagedUserResult> GetUsers(string? term, string? sort, int page, int limit);

        Task<User> GetUserByIDAsync(Guid UserID);

    }
}

