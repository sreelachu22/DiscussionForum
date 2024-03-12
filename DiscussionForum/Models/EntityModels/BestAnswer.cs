using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscussionForum.Models.EntityModels
{
    public class BestAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BestAnswerId { get; set; }
        public long ThreadID { get; set; }
        public long ReplyID { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        [ForeignKey("ThreadID")]
        public virtual Threads Thread { get; set; }
        [ForeignKey("ReplyID")]
        public virtual Reply Reply { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }
        [ForeignKey("ModifiedBy")]
        public virtual User? ModifiedByUser { get; set; }
    }
}
