using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DiscussionForum.Models.EntityModels
{
    public class ForumCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ForumCategoryID { get; set; }

        [Required(ErrorMessage = "Forum category name is required.")]
        [StringLength(100, ErrorMessage = "Forum category name cannot exceed 100 characters.")]
        public string ForumCategoryName { get; set; }
        public bool IsDeleted { get; set; }

        public ForumCategory()
        {
            // Set default values in the constructor
            IsDeleted = false;
        }
    }
}
