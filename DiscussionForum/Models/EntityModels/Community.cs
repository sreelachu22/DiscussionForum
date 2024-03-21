using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DiscussionForum.Models.EntityModels
{
    public class Community
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommunityID { get; set; }
        public string CommunityName { get; set; }
        public int CommunityStatusID { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        // Navigation properties
        [JsonIgnore]
        public virtual User CreatedByUser { get; set; }
        [JsonIgnore]
        public virtual User ModifiedByUser { get; set; }
        [JsonIgnore]
        public virtual CommunityStatus CommunityStatus { get; set; }
    }
}
