using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Services
{
    public interface ICommunityCategoryMappingService
    {
        //Task<CommunityCategoryMapping> CreateCommunityCategoryMappingAsync(CommunityCategoryMappingAPI model);
        Task<IEnumerable<CommunityCategoryMappingAPI>> GetAllCategoriesInCommunityAsync(int communityID);
        Task<IEnumerable<CommunityCategory>> GetCategoriesNotInCommunityAsync(int communityID);
        Task<CommunityCategoryMapping> GetCommunityCategoryMappingByIdAsync(int communityCategoryMappingID);
        Task<CommunityCategoryMapping> CreateCommunityCategoryMappingAsync(int communityID, string communityCategoryName,string description);
        Task<CommunityCategoryMapping> UpdateCommunityCategoryMappingAsync(int communityCategoryMappingID, string description);
        Task<CommunityCategoryMapping> DeleteCommunityCategoryMappingAsync(int communityCategoryMappingID);
    }
}
