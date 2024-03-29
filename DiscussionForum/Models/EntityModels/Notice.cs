﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscussionForum.Models.EntityModels
{
    [Table("Notices")]
    public class Notice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoticeID { get; set; }

        [Required(ErrorMessage = "CommunityID is required.")]
        public int CommunityID { get; set; }

        [Required(ErrorMessage = "Notice Title is required.")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public bool IsDeleted { get; set; }

        [ForeignKey("CreatedBy")]
        public Guid? CreatedBy { get; set; }

        [ForeignKey("ModifiedBy")]
        public Guid? ModifiedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        // Navigation properties
        [ForeignKey("CommunityID")]
        public virtual Community Community { get; set; }



        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }


        [ForeignKey("ModifiedBy")]
        public virtual User ModifiedByUser { get; set; }
    }
}
