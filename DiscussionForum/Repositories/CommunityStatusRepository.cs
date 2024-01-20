using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Repositories
{
    public class CommunityStatusRepository : Repository<CommunityStatus>, ICommunityStatusRepository
    {
        public CommunityStatusRepository(AppDbContext context) : base(context) { }
    }
}
