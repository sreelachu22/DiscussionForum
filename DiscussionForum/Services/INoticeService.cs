namespace DiscussionForum.Services
{
    using DiscussionForum.Models.EntityModels;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INoticeService
    {
        Task<IEnumerable<Notice>> GetNoticesAsync();
        Task<Notice> CreateNoticeAsync(int communityId, string title, string content, DateTime? expiresAt, Guid createdBy);
        Task<Notice> UpdateNoticeAsync(int id, int communityId, string title, string content, DateTime? expiresAt, Guid modifiedBy);
        Task<Notice> DeleteNoticeAsync(int id);
    }
}
