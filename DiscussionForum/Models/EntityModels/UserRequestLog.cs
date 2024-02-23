using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscussionForum.Models.EntityModels
{
    public class UserRequestLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserRequestLogId { get; set; }
        public long UserLogId { get; set; }
        public DateTime RequestTime { get; set; }
        public string RequestMethod { get; set; }
        public string RequestPath { get; set; }
        public string ResponseStatus { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        //Navigation properties
        public virtual UserLog UserLog { get; set; }
        public virtual User User { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual User ModifiedByUser { get; set; }
    }
}
