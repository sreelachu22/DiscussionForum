using DiscussionForum.Models.EntityModels;
using System.Text.Json.Serialization;

namespace DiscussionForum.Models.APIModels
{
    public class CommunityCategoryMappingAPI
    {
        public int CommunityCategoryMappingID { get; set; }
        public int CommunityID { get; set; }
        public long CommunityCategoryID { get; set; }

        public string? CommunityCategoryName { get; set; }
        public string Description { get; set; }

        public bool IsDeleted { get; set; }

<<<<<<< HEAD
        public Guid? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

=======
        public Guid? CreatedBy { get; set; }    
        
>>>>>>> 7cb0af06a8e5d0a7d8105c7bf3260d2f57a356d6
        public Guid? ModifiedBy { get; set; }
        public int? ThreadCount { get; set; }
    }
}
