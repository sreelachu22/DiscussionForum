namespace DiscussionForum.Models.APIModels
{
    public class CategoryThreadDto
    {
        public long ThreadID { get; set; }
        public string Content { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string CategoryName { get; set; }
        public string ThreadStatusName { get; set; }

        public Boolean IsAnswered { get; set; }
    }
}
