using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Repositories
{
    public class ReplyRepository : Repository<Reply>, IReplyRepository
    {
        public ReplyRepository(AppDbContext context) : base(context) { }

    }
}
