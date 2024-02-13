using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Type;

namespace DiscussionForum.Services
{
    public interface ILoginService
    {
        Task<TokenDto> AdminLoginAsync(AdminLoginDto adminLogin);
        Task<TokenDto> ExternalAuthenticationAsync(string token, string provider);
    }
}
