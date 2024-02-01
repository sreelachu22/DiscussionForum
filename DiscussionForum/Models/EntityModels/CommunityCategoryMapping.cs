using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DiscussionForum.Models.EntityModels
{
    public class CommunityCategoryMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommunityCategoryMappingID { get; set; }

        [Required]
        public int CommunityID { get; set; }

        [Required]
        public long CommunityCategoryID { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        // Navigation properties
        [JsonIgnore]
        [ForeignKey("CommunityID")]
        public virtual Community Community { get; set; }

        [JsonIgnore]
        [ForeignKey("CommunityCategoryID")]
        public virtual CommunityCategory CommunityCategory { get; set; }

        [JsonIgnore]
        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }

        [JsonIgnore]
        [ForeignKey("ModifiedBy")]
        public virtual User ModifiedByUser { get; set; }
    }
}
