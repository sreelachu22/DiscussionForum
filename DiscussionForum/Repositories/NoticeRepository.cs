using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Repositories
{
    public class NoticeRepository : Repository<Notice>, INoticeRepository
    {
        public NoticeRepository(AppDbContext context) : base(context) { }
    }
}
