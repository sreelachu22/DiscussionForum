using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Repositories;

namespace DiscussionForum.Services
{
    public interface ITagService
    {
        Task<Tag> CreateTagAsync(string tagname, Guid createdby);
        Task<IEnumerable<TagDto>> GetAllTagAsync(Boolean isdel);

        Task<(IEnumerable<TagDto> tagDtos, int totalPages)> GetAllPaginatedTagsAsync(bool isdel, string? sortOrder, string? searchKeyword, int pageNumber, int pageSize);

        Task<IEnumerable<TagDto>> GeAllTagAsync(String keyword);

    }
}