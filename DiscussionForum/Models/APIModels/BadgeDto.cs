using DiscussionForum.Models.EntityModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DiscussionForum.Models.APIModels
{
    public class BadgeDto
    {
        public int GoldMinScore { get; set; }
        public int SilverMinScore { get; set; }
        public int BronzeMinScore { get; set; }
    }
}
