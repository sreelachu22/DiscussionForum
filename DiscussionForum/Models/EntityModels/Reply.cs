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
        public long ThreadID { get; set; }
        public string Content { get; set; }
        public long? ParentReplyID { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("CreatedBy")]
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        [ForeignKey("ModifiedBy")]
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }              

        // Navigation properties
        [JsonIgnore]
        public virtual Threads Thread { get; set; }
        public virtual Reply ParentReply { get; set; }

        [ForeignKey("CreatedBy")]
        [JsonIgnore]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("ModifiedBy")]

        [JsonIgnore]
        public virtual User ModifiedByUser { get; set; }
    }
}
