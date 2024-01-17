using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Repositories
{
    public class ForumStatusRepository : Repository<ForumStatus>, IForumStatusRepository
    {
        public ForumStatusRepository(AppDbContext context) : base(context) { }
    }
}
