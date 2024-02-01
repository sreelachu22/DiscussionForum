using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Models.APIModels
{
    public class CommunityDTO
    {
        public int CommunityID { get; set; }
        public string CommunityName { get; set; }
        public string CommunityStatusName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int CategoryCount { get; set; }
        public int PostCount { get; set; }
        public List<string> TopCategories { get; set; }

    }
}
