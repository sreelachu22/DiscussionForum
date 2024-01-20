using DiscussionForum.Models.EntityModels;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommunityStatusController : ControllerBase
    {
        private readonly ICommunityStatusService _communityStatusService;

        public CommunityStatusController(ICommunityStatusService communityStatusService)
        {
            _communityStatusService = communityStatusService ?? throw new ArgumentNullException(nameof(communityStatusService));
        }

        // Get all community statuses.       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommunityStatus>>> GetCommunityStatus()
        {
            try
            {
                var communityStatuses = await _communityStatusService.GetCommunityStatusAsync();
                return Ok(communityStatuses);
            }
            catch (Exception ex)
            {
                throw new Exception("Internal Server Error", ex);
            }
        }

        // Get community status by ID.    
        [HttpGet("{id}")]
        public async Task<ActionResult<CommunityStatus>> GetCommunityStatusById(int id)
        {
            try
            {
                var communityStatus = await _communityStatusService.GetCommunityStatusByIdAsync(id);

                if (communityStatus == null)
                {
                    return NotFound(); // Return 404 if the resource is not found.
                }

                return Ok(communityStatus);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while fetching community status with ID {id}.", ex);
            }
        }
    }
}
