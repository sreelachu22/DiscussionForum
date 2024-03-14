using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DiscussionForum.Models.EntityModels
{
    public class Point
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PointID { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int CommunityID { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int ThreadCreated { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int ThreadUpdated { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int ThreadDeleted { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int ThreadUpvotedBy { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int ThreadUpvotedOn { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int ThreadDownvotedBy { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int ThreadDownvotedOn { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int RemoveThreadUpvoteBy { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int RemoveThreadUpvoteOn { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int RemoveThreadDownvoteBy { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int RemoveThreadDownvoteOn { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int ReplyCreated { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int ReplyUpdated { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int ReplyDeleted { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int ReplyUpvotedBy { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int ReplyUpvotedOn { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int ReplyDownvotedBy { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int ReplyDownvotedOn { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int RemoveReplyUpvoteBy { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int RemoveReplyUpvoteOn { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int RemoveReplyDownvoteBy { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public int RemoveReplyDownvoteOn { get; set; }

        [ForeignKey("CommunityID")]
        public virtual Community Community { get; set; }
    }
}
