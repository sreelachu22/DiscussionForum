using DiscussionForum.Authorization;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAngularDev")]
    public class CommunityCategoryMappingController : ControllerBase
    {
        private readonly ICommunityCategoryMappingService _communityCategoryMappingService;

        public CommunityCategoryMappingController(ICommunityCategoryMappingService communityCategoryMappingService)
        {
            _communityCategoryMappingService = communityCategoryMappingService;
        }

        /// <summary>
        /// Retrieves paginated categories inside a community based on specified parameters.
        /// </summary>
        /// <param name="communityID">The ID of the community.</param>
        /// <param name="term">The search term for filtering categories.</param>
        /// <param name="sort">The column name for sorting categories.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="limit">The limit for the number of items per page.</param>
        /// <returns>Paginated categories inside a community with total count and total pages.</returns>
        [CustomAuth("User")]
        [HttpGet]
        public async Task<IActionResult> GetAllCategories(int communityID, string term = "", string sort = "communityCategoryName", int page = 1, int limit = 10)
        {
            var categoryResult = await _communityCategoryMappingService.GetCategories(communityID, term, sort, page, limit);

            // Add pagination headers to the response
            Response.Headers.Add("X-Total-Count", categoryResult.TotalCount.ToString());
            Response.Headers.Add("X-Total-Pages", categoryResult.TotalPages.ToString());

            return Ok(categoryResult);
        }

        /// <summary>
        /// Retrieves a community category mapping by its ID.
        /// </summary>
        /// <param name="communityCategoryMappingID">The ID of the community category mapping.</param>
        /// <returns>An action result containing the community category mapping.</returns>
        [CustomAuth("Admin")]
        [HttpGet("/{communityCategoryMappingID}")]
        public async Task<ActionResult<CommunityCategoryMappingAPI>> GetCommunityCategoryMappingByIdAsync(int communityCategoryMappingID)
        {
            try
            {
                var result = await _communityCategoryMappingService.GetCommunityCategoryMappingByIdAsync(communityCategoryMappingID);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all categories mapped to a specified community.
        /// </summary>
        /// <param name="communityID">The ID of the community.</param>
        /// <returns>An action result containing the list of categories mapped to the community.</returns>
        [CustomAuth("User")]
        [HttpGet("InCommunity/{communityID}")]
        public async Task<ActionResult<IEnumerable<CommunityCategoryMappingAPI>>> GetAllCategoriesInCommunityAsync(int communityID)
        {
            try
            {
                var result = await _communityCategoryMappingService.GetAllCategoriesInCommunityAsync(communityID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all categories not mapped to a specified community.
        /// </summary>
        /// <param name="communityID">The ID of the community.</param>
        /// <returns>An action result containing the list of categories not mapped to the community.</returns>
        [CustomAuth("Admin")]
        [HttpGet("GetCategoriesNotInCommunity/{communityID}")]
        public async Task<ActionResult<IEnumerable<CommunityCategoryMappingAPI>>> GetCategoriesNotInCommunityAsync(int communityID)
        {
            try
            {
                var result = await _communityCategoryMappingService.GetCategoriesNotInCommunityAsync(communityID);

                if (result != null && result.Any())
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("No categories found for the specified community.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal Server Error");
            }
        }


        /// <summary>
        /// Creates a new community category mapping.
        /// </summary>
        /// <param name="communityID">The ID of the community.</param>
        /// <param name="model">The data for creating the community category mapping.</param>
        /// <returns>An action result containing the ID of the created mapping.</returns>
        [CustomAuth("Admin")]
        [HttpPost("CreateCategoryMapping/{communityID}")]
        public async Task<ActionResult<int>> CreateCommunityCategoryMappingAsync(int communityID, CommunityCategoryMappingAPI model)
        {
            try
            {
                var communityCategoryMapping = await _communityCategoryMappingService.CreateCommunityCategoryMappingAsync(communityID, model);
                return Ok(communityCategoryMapping);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the description of a community category mapping.
        /// </summary>
        /// <param name="communityCategoryMappingID">The ID of the community category mapping to update.</param>
        /// <param name="model">The updated data for the community category mapping.</param>
        /// <returns>An action result containing the updated community category mapping.</returns>
        [CustomAuth("Admin")]
        [HttpPut("UpdateCategoryDescription/{communityCategoryMappingID}")]
        public async Task<ActionResult<CommunityCategoryMapping>> UpdateCommunityCategoryMappingAsync(int communityCategoryMappingID, CommunityCategoryMappingAPI model)
        {
            try
            {
                var result = await _communityCategoryMappingService.UpdateCommunityCategoryMappingAsync(communityCategoryMappingID, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a community category mapping.
        /// </summary>
        /// <param name="communityCategoryMappingID">The ID of the community category mapping to delete.</param>
        /// <returns>An action result containing the deleted community category mapping.</returns>
        [CustomAuth("Admin")]
        [HttpDelete("{communityCategoryMappingID}")]
        public async Task<ActionResult<CommunityCategoryMapping>> DeleteCommunityCategoryMappingAsync(int communityCategoryMappingID)
        {
            try
            {
                var result = await _communityCategoryMappingService.DeleteCommunityCategoryMappingAsync(communityCategoryMappingID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
