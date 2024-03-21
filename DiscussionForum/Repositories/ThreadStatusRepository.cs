namespace DiscussionForum.Repositories
{
    using DiscussionForum.Data;
    using DiscussionForum.Models.EntityModels;

    public class ThreadStatusRepository : Repository<ThreadStatus>, IThreadStatusRepository
    {
        public ThreadStatusRepository(AppDbContext context) : base(context) { }
    }
}
