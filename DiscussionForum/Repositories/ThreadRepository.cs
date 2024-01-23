using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Repositories;

namespace DiscussionForum.Repositories
{
    public class ThreadRepository : Repository<Threads>, IThreadRepository
    {
        public ThreadRepository(AppDbContext context) : base(context) { }
    }
}

