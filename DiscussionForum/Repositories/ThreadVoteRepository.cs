using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Repositories
{
    public class ThreadVoteRepository : Repository<ThreadVote>, IThreadVoteRepository
    {
        public ThreadVoteRepository(AppDbContext context) : base(context) { }
    }
}
