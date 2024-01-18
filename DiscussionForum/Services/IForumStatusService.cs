using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Services
{
    public interface IForumStatusService
    {
        Task<IEnumerable<ForumStatus>> GetForumStatusAsync();
        Task<ForumStatus> GetForumStatusByIdAsync(int id);
    }
}
