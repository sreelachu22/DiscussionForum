using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Services
{
    public interface ICommunityService
    {
        Task<IEnumerable<CommunityDTO>> GetAllCommunitiesAsync();
    }
}
