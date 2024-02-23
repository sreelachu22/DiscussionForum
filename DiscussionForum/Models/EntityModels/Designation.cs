using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscussionForum.Models.EntityModels
{
    public class Designation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DesignationID { get; set; }
        public string DesignationName { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public bool IsDeleted { get; set; }

    }

}
