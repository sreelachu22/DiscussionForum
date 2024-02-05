using System.ComponentModel.DataAnnotations;

namespace DiscussionForum.Models.EntityModels
{
    public class User
    {
        [Key]
        public Guid UserID { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; }

        public long? Score { get; set; }

        [Required(ErrorMessage = "Department ID required.")]
        public int DepartmentID { get; set; }

        public virtual Department Department { get; set; }

        [Required(ErrorMessage = "Designation ID required.")]
        public long DesignationID { get; set; }

        public virtual Designation Designation { get; set; }

        public bool IsDeleted { get; set; }

        //[Required(ErrorMessage = "Created User is required.")]
        public Guid? CreatedBy { get; set; }

        public virtual User CreatedByUser { get; set; }

        [Required(ErrorMessage = "Created User date is required.")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Guid? ModifiedBy { get; set; }

        public virtual User ModifiedByUser { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public virtual ICollection<ThreadVote> ThreadVotesCreatedBy { get; set; }
        public virtual ICollection<ThreadVote> ThreadVotesModifiedBy { get; set; }
        public virtual ICollection<ReplyVote> ReplyVotesCreatedBy { get; set; }
        public virtual ICollection<ReplyVote> ReplyVotesModifiedBy { get; set; }


    }
}