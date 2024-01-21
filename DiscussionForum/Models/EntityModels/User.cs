﻿using System.ComponentModel.DataAnnotations;

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

        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }

        public long DesignationID { get; set; }
        public virtual Designation Designation { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? CreatedBy { get; set; }
        public virtual User CreatedByUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Guid? ModifiedBy { get; set; }
        public virtual User ModifiedByUser { get; set; }

        public DateTime? ModifiedAt { get; set; }
    }
}