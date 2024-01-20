using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DiscussionForum.Models.EntityModels
{
    public class CommunityCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CommunityCategoryID { get; set; }

        [Required(ErrorMessage = "Community category name is required.")]
        [StringLength(100, ErrorMessage = "Community category name cannot exceed 100 characters.")]
        public string CommunityCategoryName { get; set; }
        public bool IsDeleted { get; set; }

        public CommunityCategory()
        {
            // Set default values in the constructor
            IsDeleted = false;
        }
    }
}
