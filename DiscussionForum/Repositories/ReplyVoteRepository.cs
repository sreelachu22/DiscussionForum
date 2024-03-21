using DiscussionForum.Data;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Repositories
{
    public class ReplyVoteRepository : Repository<ReplyVoteDto>, IReplyVoteRepository
    {
        public ReplyVoteRepository(AppDbContext context) : base(context) { }
    }
}
