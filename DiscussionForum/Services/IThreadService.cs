using DiscussionForum.Models.EntityModels;
using DiscussionForum.Models.APIModels;

namespace DiscussionForum.Services
{
    public interface IThreadService
    {
        Task<IEnumerable<CategoryThreadDto>> GetAllThreads(int CommunityCategoryMappingID);
    }
}
