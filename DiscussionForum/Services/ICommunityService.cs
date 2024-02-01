using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Services
{
    public interface ICommunityService
    {
        /// <summary>
        /// Retrieves all communities.
        /// </summary>
        Task<IEnumerable<CommunityDTO>> GetAllCommunitiesAsync();
    }
}
