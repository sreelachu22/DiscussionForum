using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Repositories;

namespace DiscussionForum.Repositories
{
    public class CommunityRepository : Repository<Community>, ICommunityRepository
    {
        public CommunityRepository(AppDbContext context) : base(context) { }
    }
}
