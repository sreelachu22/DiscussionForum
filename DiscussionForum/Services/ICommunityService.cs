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
        /// <summary>
        /// Retrieves a specific community.
        /// </summary>
        /// <param name="communityID">The ID of the community to be retrieved</param>
        Task<CommunityDTO> GetCommunityByIdAsync(int communityID);
    }
}
