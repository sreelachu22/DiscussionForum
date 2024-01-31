using DiscussionForum.Models.APIModels;

namespace DiscussionForum.Services
{
    using DiscussionForum.Models.EntityModels;
    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<PagedUserResult> GetUsers(string? term, string? sort, int page, int limit);

        Task<SingleUserDTO> GetUserByIDAsync(Guid UserID);

        Task<String> PutUserByIDAsync(Guid userId, int roleID, Guid adminId);

    }
}

