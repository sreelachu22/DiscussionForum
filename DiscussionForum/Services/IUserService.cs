using DiscussionForum.Models.APIModels;

namespace DiscussionForum.Services
{
    public interface IUserService
    {
        Task<PagedUserResult> GetUsers(string? term, string? sort, int page, int limit);
    }
}
