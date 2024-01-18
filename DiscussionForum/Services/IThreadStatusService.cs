//using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
namespace DiscussionForum.Services
{
    public interface IThreadStatusService
    {
        Task <IEnumerable<ThreadStatus>> GetThreadStatusAsync();
        Task<ThreadStatus> GetThreadStatusByIdAsync(int id);
    }
}