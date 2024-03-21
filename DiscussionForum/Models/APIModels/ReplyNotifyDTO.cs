namespace DiscussionForum.Models.APIModels
{
    public class ReplyNotifyDTO
    {
        public long ChildReplyID { get; set; }
        public string ChildReplyContent { get; set; }
        public DateTime ChildReplyCreatedAt { get; set; }
        public string ChildReplyUserName { get; set; }
        public long? ParentReplyID { get; set; }
        public string ParentReplyUserName { get; set; }
        public string ParentReplyContent { get; set; }
        public string CategoryName { get; set; }
        public string CommunityName { get; set; }
        public string ThreadContent { get; set; }
    }
}
