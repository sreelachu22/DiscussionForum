using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Models.APIModels
{
    //DTO for generating all the replies of a thread in a nested reply manner.
    //Each reply will have its child replies stored in NestedReplies list.
    public class ReplyDTO
    {
        public long ReplyID { get; set; }

        public long ThreadID { get; set; }
        public long? ParentReplyID { get; set; }
        public string Content { get; set; }
        public int UpvoteCount { get; set; }
        public int DownvoteCount { get; set; }
        public bool IsDeleted { get; set; }
        public bool? HasViewed { get; set; }
        public Guid? CreatedBy { get; set; }
        public string? CreatedUserName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ThreadOwnerEmail { get; set; }
        public List<ReplyDTO> NestedReplies { get; set; }
    }
}
