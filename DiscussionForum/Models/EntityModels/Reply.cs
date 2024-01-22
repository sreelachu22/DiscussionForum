using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DiscussionForum.Models.EntityModels
{
    public class Reply
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReplyID { get; set; }
        public long ThreadID { get; set; }
        public string Content { get; set; }
        public long ParentReplyID { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        // Navigation properties
        public virtual Threads Thread { get; set; }
        public virtual Reply ParentReply { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual User ModifiedByUser { get; set; }
    }
}
