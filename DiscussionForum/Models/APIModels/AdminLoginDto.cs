using System.ComponentModel.DataAnnotations;

namespace DiscussionForum.Models.APIModels
{
    public class AdminLoginDto
    {
        [Required(ErrorMessage = "UserMail is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "UserMail is required.")]
        public string Password { get; set; }
    }
}
