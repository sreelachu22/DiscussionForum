using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DiscussionForum.Models.EntityModels
{
    public class Threads
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ThreadID { get; set; }
        public int? CommunityCategoryMappingID { get; set; }
        public string Content { get; set; }
        public int? ThreadStatusID { get; set; }
        public bool IsAnswered { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        // Navigation properties
        public virtual CommunityCategoryMapping CommunityCategoryMapping { get; set; }
        public virtual ThreadStatus ThreadStatus { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual User ModifiedByUser { get; set; }
    }
}
