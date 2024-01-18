using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Repositories;

namespace DiscussionForum.Repositories
{
    public class ForumCategoryRepository : Repository<ForumCategory>, IForumCategoryRepository
    {
        public ForumCategoryRepository(AppDbContext context) : base(context) { }
    }
}
