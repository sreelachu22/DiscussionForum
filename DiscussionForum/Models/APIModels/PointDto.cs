namespace DiscussionForum.Models.APIModels
{
    public class PointDto
    {
        public int ThreadCreated { get; set; }
        public int ThreadUpdated { get; set; }
        public int ThreadDeleted { get; set; }
        public int ThreadUpvotedBy { get; set; }
        public int ThreadUpvotedOn { get; set; }
        public int ThreadDownvotedBy { get; set; }
        public int ThreadDownvotedOn { get; set; }
        public int RemoveThreadUpvoteBy { get; set; }
        public int RemoveThreadUpvoteOn { get; set; }
        public int RemoveThreadDownvoteBy { get; set; }
        public int RemoveThreadDownvoteOn { get; set; }
        public int ReplyCreated { get; set; }
        public int ReplyUpdated { get; set; }
        public int ReplyDeleted { get; set; }
        public int ReplyUpvotedBy { get; set; }
        public int ReplyUpvotedOn { get; set; }
        public int ReplyDownvotedBy { get; set; }
        public int ReplyDownvotedOn { get; set; }
        public int RemoveReplyUpvoteBy { get; set; }
        public int RemoveReplyUpvoteOn { get; set; }
        public int RemoveReplyDownvoteBy { get; set; }
        public int RemoveReplyDownvoteOn { get; set; }
    }
}
