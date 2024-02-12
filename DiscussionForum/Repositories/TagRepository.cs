using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Repositories
{
    public class TagRepository : Repository<Tag>,ITagRepository
    {
        public TagRepository(AppDbContext context) : base(context) { }
    }
}
