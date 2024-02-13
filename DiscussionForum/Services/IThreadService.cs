using DiscussionForum.Models.EntityModels;
using DiscussionForum.Models.APIModels;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Services
{
    public interface IThreadService
    {
        /// <summary>
        /// Retrieves all threads in a category.
        /// </summary>
        /// <param name="CommunityCategoryMappingID">The mapping ID of the category in a community whose threads must be fetched.</param>
        Task<(IEnumerable<CategoryThreadDto> Threads, int TotalCount, string CategoryName, string CategoryDescription)> GetAllThreads(int CommunityCategoryMappingID, int pageNumber, int pageSize);
        /// <summary>
        /// Retrieves a thread based on the given thread ID.
        /// </summary>
        /// <param name="threadId">The ID of the thread to search for in threads.</param>
        Task<CategoryThreadDto> GetThreadByIdAsync(long threadId);
        /// <summary>
        /// Creates a new thread with content from request body.
        /// </summary>
        /// <param name="communityCategoryMappingId">The mapping ID of the category in a community where threads must be posted.</param>
        /// <param name="creatorId">The ID of the user posting the thread.</param>
        /// <param name="content">The content of the thread.</param>
        Task<Threads> CreateThreadAsync(CategoryThreadDto categorythreaddto,int communityCategoryId, Guid createdby);
        /// <summary>
        /// Updates a thread with content from request body based on the given thread ID.
        /// </summary>
        /// <param name="threadId">The ID of the thread to be updated.</param>
        /// <param name="modifierId">The ID of the user editing the thread.</param>
        /// <param name="content">The content of the thread.</param>
        Task<Threads> UpdateThreadAsync(long threadId, Guid modifierId, string? title, string? content);
        /// <summary>
        /// Deletes a thread based on the given thread ID.
        /// </summary>
        /// <param name="threadId">The ID of the thread to be deleted.</param>
        /// <param name="modifierId">The ID of the user deleting the thread.</param>
        Task<Threads> DeleteThreadAsync(long threadId, Guid modifierId);
        /// <summary>
        /// Retrieves all threads.
        /// </summary>
        Task<IEnumerable<Threads>> GetThreadsFromDatabaseAsync();
        Task<IEnumerable<CategoryThreadDto>> GetTopThreads(int CommunityCategoryMappingID, string sortBy, int topCount);
        Task<(IEnumerable<CategoryThreadDto> Threads, int TotalCount, string CommunityName)> GetClosedThreads(int CommunityID, int pageNumber, int pageSize);
    }
}

