namespace DiscussionForum.Models.APIModels
{
    public class ThreadVoteDto
    {
        public Guid UserID { get; set; }
        public long ThreadID { get; set; }
        public bool IsUpVote { get; set; }
    }
}
