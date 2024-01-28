using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Services
{
    public interface ICommunityCategoryMappingService
    {
        //Task<CommunityCategoryMapping> CreateCommunityCategoryMappingAsync(CommunityCategoryMappingAPI model);
        Task<PagedCategoryResult> GetCategories(int communityID, string? term, string? sort, int page, int limit);
        Task<IEnumerable<CommunityCategoryMappingAPI>> GetAllCategoriesInCommunityAsync(int communityID);
        Task<IEnumerable<CommunityCategory>> GetCategoriesNotInCommunityAsync(int communityID);
        Task<CommunityCategoryMapping> GetCommunityCategoryMappingByIdAsync(int communityCategoryMappingID);
        Task<CommunityCategoryMapping> CreateCommunityCategoryMappingAsync(int communityID, CommunityCategoryMappingAPI model);
        Task<CommunityCategoryMapping> UpdateCommunityCategoryMappingAsync(int communityCategoryMappingID, CommunityCategoryMappingAPI model);
        Task<CommunityCategoryMapping> DeleteCommunityCategoryMappingAsync(int communityCategoryMappingID);
    }
}
