using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DiscussionForum.Models.EntityModels
{
    public class User
    {
        [Key]
        public Guid UserID { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; }

        public long? Score { get; set; }

        [ForeignKey("DepartmentID")]
        [Required(ErrorMessage = "Department ID required.")]
        public int? DepartmentID { get; set; }

        public virtual Department Department { get; set; }

        [ForeignKey("DesignationID")]
        [Required(ErrorMessage = "Designation ID required.")]
        public long? DesignationID { get; set; }

        public virtual Designation Designation { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? CreatedBy { get; set; }

        public User CreatedByUser { get; set; }

        [Required(ErrorMessage = "Created User date is required.")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Guid? ModifiedBy { get; set; }

        public User ModifiedByUser { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [JsonIgnore]
        [InverseProperty("CreatedByUser")]
        public virtual ICollection<ThreadVote> ThreadVotesCreatedBy { get; set; }

        [JsonIgnore]
        [InverseProperty("ModifiedByUser")]
        public virtual ICollection<ThreadVote> ThreadVotesModifiedBy { get; set; }

        [JsonIgnore]
        [InverseProperty("CreatedByUser")]
        public virtual ICollection<ReplyVote> ReplyVotesCreatedBy { get; set; }

        [JsonIgnore]
        [InverseProperty("ModifiedByUser")]
        public virtual ICollection<ReplyVote> ReplyVotesModifiedBy { get; set; }
    }
}
