using DiscussionForum.Models.EntityModels;
using DiscussionForum.Models.APIModels;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Services
{
    public interface IThreadService
    {
        Task<(IEnumerable<CategoryThreadDto> Threads, int TotalCount, string CategoryName, string CategoryDescription)> GetAllThreads(int CommunityCategoryMappingID, int pageNumber, int pageSize);
        Task<IEnumerable<Threads>> GetThreadsFromDatabaseAsync();
    }

}
