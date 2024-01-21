using DiscussionForum.Models.EntityModels;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    [EnableCors("AllowAngularDev")]
    [ApiController]
    [Route("api/[controller]")]
    public class ForumStatusController : ControllerBase
    {
        private readonly IForumStatusService _forumStatusService;

        public ForumStatusController(IForumStatusService forumStatusService)
        {
            _forumStatusService = forumStatusService ?? throw new ArgumentNullException(nameof(forumStatusService));
        }

        // Get all forum statuses.       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ForumStatus>>> GetForumStatus()
        {
            try
            {
                var forumStatuses = await _forumStatusService.GetForumStatusAsync();
                return Ok(forumStatuses);
            }
            catch (Exception ex)
            {
                throw new Exception("Internal Server Error", ex);
            }
        }

        // Get forum status by ID.    
        [HttpGet("{id}")]
        public async Task<ActionResult<ForumStatus>> GetForumStatusById(int id)
        {
            try
            {
                var forumStatus = await _forumStatusService.GetForumStatusByIdAsync(id);

                if (forumStatus == null)
                {
                    return NotFound(); // Return 404 if the resource is not found.
                }

                return Ok(forumStatus);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching forum category with ID {id}.", ex);
            }
        }
    }
}
