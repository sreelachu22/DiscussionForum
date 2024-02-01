using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DiscussionForum.Models.EntityModels
{
    public class Threads
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ThreadID { get; set; }

        [Required(ErrorMessage = "Community Category Mapping ID is required.")]
        public int CommunityCategoryMappingID { get; set; }

        [Required(ErrorMessage = "Post Content is required.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Thread Status ID is required.")]
        public int ThreadStatusID { get; set; }

        [Required(ErrorMessage = "IsAnswered required.")]
        public bool IsAnswered { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        // Navigation properties
        public virtual CommunityCategoryMapping CommunityCategoryMapping { get; set; }
        public virtual ThreadStatus ThreadStatus { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual User ModifiedByUser { get; set; }
        public virtual ICollection<ThreadVote> ThreadVotes { get; set; }

    }
}
