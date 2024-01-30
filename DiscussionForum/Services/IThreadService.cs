using DiscussionForum.Models.EntityModels;
using DiscussionForum.Models.APIModels;

namespace DiscussionForum.Services
{
    public interface IThreadService
    {
        Task<(IEnumerable<CategoryThreadDto> Threads, int TotalCount, string CategoryName, string CategoryDescription)> GetAllThreads(int CommunityCategoryMappingID, int pageNumber, int pageSize);
        Task<Threads> GetThreadByIdAsync(long threadId);
        Task<Threads> CreateThreadAsync(int communityCategoryMappingId, Guid creatorId, string content);
        Task<Threads> UpdateThreadAsync(long threadId, Guid modifierId, string content);
        Task DeleteThreadAsync(long threadId, Guid modifierId);
    }
}
