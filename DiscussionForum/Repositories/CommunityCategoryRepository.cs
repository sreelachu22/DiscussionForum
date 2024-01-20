using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Repositories;

namespace DiscussionForum.Repositories
{
    public class CommunityCategoryRepository : Repository<CommunityCategory>, ICommunityCategoryRepository
    {
        public CommunityCategoryRepository(AppDbContext context) : base(context) { }
    }
}
