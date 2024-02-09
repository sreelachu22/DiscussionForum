using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Repositories;

namespace DiscussionForum.Services
{
    public interface ITagService
    {
        Task<Tag> CreateTagAsync(string tagname, Guid createdby);
        Task<IEnumerable<Tag>> GetAllTagAsync(Boolean isdel);

    }
}
