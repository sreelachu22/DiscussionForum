using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Repositories
{
    public class DesignationRepository : Repository<Designation>, IDesignationRepository
    {
        public DesignationRepository(AppDbContext context) : base(context) { }

    }

}
