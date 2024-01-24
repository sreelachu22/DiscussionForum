using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Services
{
    public interface ICommunityService
    {
        Task<IEnumerable<Community>> GetAllCommunities();
    }
}
