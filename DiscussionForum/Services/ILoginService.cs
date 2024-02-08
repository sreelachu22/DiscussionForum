using DiscussionForum.Models.APIModels;
using DiscussionForum.Type;

namespace DiscussionForum.Services
{
    public interface ILoginService
    {
        Task<ServiceResponse<string>> AdminLoginAsync(LoginDto dto);

        /*Task<ServiceResponse<string>> ExternalAuthenticationAsync(string token, string provider);*/
    }
}
