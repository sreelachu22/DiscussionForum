using DiscussionForum.Models.APIModels;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAngularDev")]
    public class CommunityController : ControllerBase
    {
        private readonly ICommunityService _communityService;

        public CommunityController(ICommunityService communityService)
        {
            _communityService = communityService;
        }

        /// <summary>
        /// Retrieves all communities.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCommunities()
        {
            try
            {
                IEnumerable<CommunityDTO> _communities = await _communityService.GetAllCommunitiesAsync();
                return Ok(_communities);
            }
            catch (Exception ex)
            {
                //Checks for an inner exception and returns corresponding error message
                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"Error while retrieving all communities \nError: {ex.InnerException.Message}");
                }
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Error while retrieving all communities \nError: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a specific community.
        /// </summary>
        /// <param name="communityId">The ID of the community to be retrieved</param>
        [HttpGet("{communityId}")]
        public async Task<IActionResult> GetCommunityById(int communityId)
        {
            try
            {
                //Validates the request data
                if (communityId <= 0)
                {
                    throw new Exception("Invalid communityId. It should be greater than zero.");
                }

                CommunityDTO _community = await _communityService.GetCommunityByIdAsync(communityId);

                //Checks if the retrieved community is null
                if (_community == null)
                {
                    throw new Exception($"Community with ID {communityId} not found.");
                }

                return Ok(_community);

            }
            catch (Exception ex)
            {
                //Checks for an inner exception and returns corresponding error message
                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"Error while retrieving community with ID = {communityId} \nError: {ex.InnerException.Message}");
                }
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Error while retrieving community with ID = {communityId} \nError: {ex.Message}");
            }
        }

    }

}
