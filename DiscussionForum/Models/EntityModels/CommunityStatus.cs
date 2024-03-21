using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscussionForum.Models.EntityModels
{
    public class CommunityStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommunityStatusID { get; set; }

        [Required]
        [StringLength(20)]
        public string CommunityStatusName { get; set; }
    }
}
