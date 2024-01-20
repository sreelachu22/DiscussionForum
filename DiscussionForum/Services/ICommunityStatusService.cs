using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Services
{
    public interface ICommunityStatusService
    {
        Task<IEnumerable<CommunityStatus>> GetCommunityStatusAsync();
        Task<CommunityStatus> GetCommunityStatusByIdAsync(int id);
    }
}
