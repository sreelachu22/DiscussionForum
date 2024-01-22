using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Models.APIModels
{
    public class PagedUserResult
    {
        public IReadOnlyCollection<User> Users { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
