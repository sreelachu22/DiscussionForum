using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DiscussionForum.Models.EntityModels
{
    public class SavedPosts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SavedPostID { get; set; }

        [ForeignKey("UserID")]
        public Guid UserID { get; set; }

        [ForeignKey("ThreadID")]
        public long ThreadID { get; set; }
        public DateTime SavedAt { get; set; }

        // Navigation properties
        [JsonIgnore]
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual Threads Thread { get; set; }
    }
}
