using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DiscussionForum.Models.EntityModels
{
    public class Tag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TagID { get; set; }
        public string TagName { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        // Navigation property to represent the relationship with the 'Users' table
        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual User ModifiedByUser { get; set; }
    }
}
