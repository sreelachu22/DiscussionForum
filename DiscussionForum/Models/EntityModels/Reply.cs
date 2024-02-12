using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DiscussionForum.Models.APIModels;

namespace DiscussionForum.Models.EntityModels
{
    public class Reply
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReplyID { get; set; }
        [ForeignKey("ThreadID")]
        public long ThreadID { get; set; }
        public string Content { get; set; }
        [ForeignKey("ParentReplyID")]
        public long? ParentReplyID { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("CreatedBy")]
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("ModifiedBy")]
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool? HasViewed { get; set; } 

        // Navigation properties
        [ForeignKey("ThreadID")]
        [JsonIgnore]
        public virtual Threads Threads { get; set; }
        [ForeignKey("ParentReplyID")]
        public virtual Reply ParentReply { get; set; }

        [ForeignKey("CreatedBy")]
        [JsonIgnore]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("ModifiedBy")]
        [JsonIgnore]
        public virtual User ModifiedByUser { get; set; }
        public virtual ICollection<ReplyVote> ReplyVotes { get; set; }
    }
}
