using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Repositories;

namespace DiscussionForum.Repositories
{
    public class CommunityCategoryMappingRepository : Repository<CommunityCategoryMapping>, ICommunityCategoryMappingRepository
    {
        public CommunityCategoryMappingRepository(AppDbContext context) : base(context) { }
    }
}
