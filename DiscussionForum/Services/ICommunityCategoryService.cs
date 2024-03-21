namespace DiscussionForum.Services
{
    using DiscussionForum.Models.EntityModels;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICommunityCategoryService
    {
        Task<IEnumerable<CommunityCategory>> GetCommunityCategoriesAsync();
        Task<CommunityCategory> GetCommunityCategoryByIdAsync(long id);
        Task<CommunityCategory> CreateCommunityCategoryAsync(string communityCategoryName);
        Task<CommunityCategory> UpdateCommunityCategoryAsync(long id, CommunityCategory communityCategoryDto);
        Task<CommunityCategory> DeleteCommunityCategoryAsync(long id);
    }
}
