using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscussionForum.Models.EntityModels
{
    public class ForumStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ForumStatusID { get; set; }

        [Required]
        [StringLength(20)]
        public string ForumStatusName { get; set; }
    }
}
