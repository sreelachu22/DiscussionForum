using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscussionForum.Models.EntityModels
{
    public class Badge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BadgeID { get; set; }

        [Required]
        public int CommunityID { get; set; }

        [Required]
        public int GoldMinScore { get; set; }

        [Required]
        public int SilverMinScore { get; set; }

        [Required]
        public int BronzeMinScore { get; set; }

        [ForeignKey("CommunityID")]
        public virtual Community Community { get; set; }
    }
}
