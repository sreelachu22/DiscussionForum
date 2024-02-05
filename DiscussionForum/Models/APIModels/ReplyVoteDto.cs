namespace DiscussionForum.Models.APIModels
{
    public class ReplyVoteDto
    {
        public Guid UserID { get; set; }
        public long ReplyID { get; set; }
        public bool IsUpVote { get; set; }
    }
}
