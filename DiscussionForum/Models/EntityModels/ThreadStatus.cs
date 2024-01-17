using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DiscussionForum.Models.EntityModels
{
    public class ThreadStatus
    {
        public int ThreadStatusID { get; set; }
      
        public string ThreadStatusName { get; set; }
    }
}
