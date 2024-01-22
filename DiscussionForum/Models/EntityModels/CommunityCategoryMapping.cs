using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DiscussionForum.Models.EntityModels
{
    public class CommunityCategoryMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommunityCategoryMappingID { get; set; }
        public int CommunityID { get; set; }
        public long CommunityCategoryID { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }

        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        // Navigation properties
        [JsonIgnore]
        public virtual Community Community { get; set; }
        [JsonIgnore]
        public virtual CommunityCategory CommunityCategory { get; set; }
        [JsonIgnore]
        public virtual User CreatedByUser { get; set; }
        [JsonIgnore]
        public virtual User ModifiedByUser { get; set; }
    }
}
