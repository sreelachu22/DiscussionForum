using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Services
{
    public interface IThreadService
    {
        Task<IEnumerable<Threads>> GetAllThreads(int CommunityCategoryMappingID);
    }
}
