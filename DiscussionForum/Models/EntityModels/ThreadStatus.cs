using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DiscussionForum.Models.EntityModels
{
    public class ThreadStatus
    {
        public int ThreadStatusID { get; set; }

        [Required(ErrorMessage = "ThreadStatusName is required.")]
        public string ThreadStatusName { get; set; }
    }
}
