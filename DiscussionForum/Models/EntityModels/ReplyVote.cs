using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DiscussionForum.Models.EntityModels
{
    public class ReplyVote
<<<<<<< HEAD
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReplyVoteID { get; set; }
        [Required]
        public Guid? UserID { get; set; }
        [Required]
        public long ReplyID { get; set; }
        [Required]
        public bool IsUpVote { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
=======
     {
         [Key]
         [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         public int ReplyVoteID { get; set; }
         [ForeignKey("UserID")]
         public Guid? UserID { get; set; }
         [ForeignKey("ReplyID")]
         public long ReplyID { get; set; }
         public bool IsUpVote { get; set; }
         public bool IsDeleted { get; set; }
         [ForeignKey("CreatedBy")]
         public Guid? CreatedBy { get; set; }
         public DateTime? CreatedAt { get; set; }
         [ForeignKey("ModifiedBy")]
         public Guid? ModifiedBy { get; set; }
         public DateTime? ModifiedAt { get; set; }
>>>>>>> c2dbaf4fd3d98e5e934025f00ec098a52b877908

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
