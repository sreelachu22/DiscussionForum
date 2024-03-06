using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Services
{
    public interface ISavedPostService
    {
        Task<int> SavePost(SavedPostDTO savedPostDto);
        Task DeleteSavedPost(Guid userId, int threadId);
        Task<List<SavedPosts>> GetSavedPostsByUserId(Guid userId);
    }
}
