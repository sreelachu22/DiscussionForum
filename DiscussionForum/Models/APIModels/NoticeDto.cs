namespace DiscussionForum.Models.APIModels
{
    public class NoticeDto
    {
        public int CommunityID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public Guid CreatedBy { get; set; }

        public Guid ModifiedBy { get; set; }
    }
}
