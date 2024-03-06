namespace DiscussionForum.Models.APIModels
{
    public class ReplyVoteDto
    {
        public Guid UserID { get; set; }
        public long ReplyID { get; set; }
        public bool IsUpVote { get; set; }
        public bool IsDeleted { get; set; }

        public int UpvoteCount { get; set; }
        public int DownvoteCount { get; set; }
    }
}
