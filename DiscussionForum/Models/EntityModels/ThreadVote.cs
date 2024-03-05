using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscussionForum.Models.EntityModels
{
    public class ThreadVote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ThreadVoteID { get; set; }
        [Required]
        public Guid UserID { get; set; }
        [Required]
        public long ThreadID { get; set; }
        [Required]
        public bool IsUpVote { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Threads Thread { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual User ModifiedByUser { get; set; }
    }
}
