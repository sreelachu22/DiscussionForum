using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DiscussionForum.Models.EntityModels
{
    public class ReplyVote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReplyVoteID { get; set; }
        [ForeignKey("UserID")]
        [Required]
        public Guid UserID { get; set; }
        [ForeignKey("ReplyID")]
        [Required]
        public long ReplyID { get; set; }
        [Required]
        public bool IsUpVote { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("CreatedBy")]
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("ModifiedBy")]
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        [JsonIgnore]
        public virtual User User { get; set; }
        [ForeignKey("ReplyID")]
        [JsonIgnore]
        public virtual Reply Reply { get; set; }
        [ForeignKey("CreatedBy")]
        [JsonIgnore]
        public virtual User CreatedByUser { get; set; }
        [ForeignKey("ModifiedBy")]
        [JsonIgnore]
        public virtual User ModifiedByUser { get; set; }
    }
}
