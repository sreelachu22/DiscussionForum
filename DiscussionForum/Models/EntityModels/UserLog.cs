using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DiscussionForum.Models.EntityModels
{
    public class UserLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserLogID { get; set; }
        public Guid UserID { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual User ModifiedByUser { get; set; }
    }
}
