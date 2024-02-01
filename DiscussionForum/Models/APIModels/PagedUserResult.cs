using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Models.APIModels
{
    //DTO to display the users after paginating them according to the values provided
    public class PagedUserResult
    {
        //elements cannot be added, removed, or modified after the collection is created.
        public IReadOnlyCollection<User> Users { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
