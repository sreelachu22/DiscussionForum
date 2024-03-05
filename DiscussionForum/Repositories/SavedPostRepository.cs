using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Repositories;

namespace DiscussionForum.Repositories
{
    public class SavedPostRepository : Repository<SavedPosts>, ISavedPostRepository
    {
        public SavedPostRepository(AppDbContext context) : base(context) { }
    }
}

