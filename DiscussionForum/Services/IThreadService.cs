using DiscussionForum.Models.EntityModels;
using DiscussionForum.Models.APIModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace DiscussionForum.Services
{
    public interface IThreadService
    {
        /// <summary>
        /// Retrieves all threads in a category.
        /// </summary>
        /// <param name="CommunityCategoryMappingID">The mapping ID of the category in a community whose threads must be fetched.</param>
        Task<(IEnumerable<CategoryThreadDto> Threads, int TotalCount, string CategoryName, string CategoryDescription)> GetAllThreads(int CommunityCategoryMappingID, int pageNumber, int pageSize,int filterOption,int sortOption);
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
        Task<Threads> CreateThreadAsync(CategoryThreadDto categorythreaddto, int communityCategoryId, Guid createdby);
        /// <summary>
        /// Updates a thread with content from request body based on the given thread ID.
        /// </summary>
        /// <param name="threadId">The ID of the thread to be updated.</param>
        /// <param name="modifierId">The ID of the user editing the thread.</param>
        /// <param name="content">The content of the thread.</param>
        Task<Threads> UpdateThreadAsync(long threadId, Guid modifierId, string? title, string? content);
        Task<Threads> CloseThreadAsync(long threadId, Guid modifierId);
        Task<Threads> ReopenThreadAsync(long threadId, Guid modifierId);
        /// <summary>
        /// Deletes a thread based on the given thread ID.
        /// </summary>
        /// <param name="threadId">The ID of the thread to be deleted.</param>
        /// <param name="modifierId">The ID of the user deleting the thread.</param>
        Task<Threads> DeleteThreadAsync(long threadId, Guid modifierId);
        /// <summary>
        /// Retrieves all threads.
        /// </summary>
        Task<(IEnumerable<CategoryThreadDto> SearchThreadDtoList, int SearchThreadDtoListLength, bool isSearchTag)> ThreadTitleSearch(string searchTerm, int pageNumber, int pageSize);

        Task<(IEnumerable<TagDto> SearchTagList, bool isSearchTag)> ThreadTagSearch(string searchTerm);

        Task<(IEnumerable<CategoryThreadDto> threadDtoList, int threadDtoListCount)> DisplaySearchedThreads(string searchTerm, int pageNumber, int pageSize,int filterOption,int sortOption);
        Task<IEnumerable<CategoryThreadDto>> GetTopThreads(int CommunityCategoryMappingID, string sortBy, int topCount);
        Task<(IEnumerable<CategoryThreadDto> Threads, int TotalCount, string CommunityName)> GetClosedThreads(int CommunityID, int pageNumber, int pageSize);
        Task<(IEnumerable<CategoryThreadDto> Threads, int TotalCount, string CategoryName, string CategoryDescription)> GetMyThreads(Guid userId, int pageNumber, int pageSize, int filterOption, int sortOption);
        Task<long> GetOriginalThreadIdAsync(long threadId);
        Task<DuplicateThreads> MarkDuplicateThreadAsync(long duplicateThreadId, long originalThreadId, Guid createdBy);
    }
}

