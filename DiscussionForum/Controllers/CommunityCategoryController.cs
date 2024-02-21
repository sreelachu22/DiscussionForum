using DiscussionForum.Models.EntityModels;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscussionForum.Controllers
{
    [ApiController]
    [EnableCors("AllowAngularDev")]
    [Route("api/[controller]")]
    public class CommunityCategoryController : ControllerBase
    {
        private readonly ICommunityCategoryService _communityCategoryService;

        public CommunityCategoryController(ICommunityCategoryService communityCategoryService)
        {
            _communityCategoryService = communityCategoryService;
        }

        
        /// <summary>
        /// Get all categories in the discussion forum (created by super admin.
        /// Community head will use these categories inside the community for community-category mapping.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCommunityCategories()
        {
            try
            {
                var communityCategories = await _communityCategoryService.GetCommunityCategoriesAsync();
                return Ok(communityCategories);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// Get a community category by id
        /// </summary>
        /// <param name="communityCategoryId"></param>
        /// <returns></returns>

        [HttpGet("{communityCategoryId}")]
        public async Task<IActionResult> GetCommunityCategoryById(long communityCategoryId)
        {
            try
            {
                var communityCategory = await _communityCategoryService.GetCommunityCategoryByIdAsync(communityCategoryId);

                if (communityCategory == null)
                    return NotFound();

                return Ok(communityCategory);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// Create a new communityCategory by giving communitycategoryName
        /// </summary>
        /// <param name="communityCategoryName"></param>
        /// <returns></returns>
        [HttpPost("{communityCategoryName}")]
        public async Task<IActionResult> CreateCommunityCategory(string communityCategoryName)
        {
            try
            {
                var communityCategory = await _communityCategoryService.CreateCommunityCategoryAsync(communityCategoryName);
                return Ok(communityCategory);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// Updates an existing community category.
        /// </summary>
        /// <param name="id">The ID of the community category to update.</param>
        /// <param name="communityCategoryDto">The updated community category data.</param>
        /// <returns>An action result containing the updated community category.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCommunityCategory(long id, [FromBody] CommunityCategory communityCategoryDto)
        {
            try
            {
                var updatedCommunityCategory = await _communityCategoryService.UpdateCommunityCategoryAsync(id, communityCategoryDto);

                if (updatedCommunityCategory == null)
                    return NotFound();

                return Ok(updatedCommunityCategory);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// Deletes a community category.
        /// </summary>
        /// <param name="communityCategoryId">The ID of the community category to delete.</param>
        /// <returns>An action result indicating the success of the operation.</returns>
        [HttpDelete("{communityCategoryId}")]
        public async Task<IActionResult> DeleteCommunityCategory(long communityCategoryId)
        {
            try
            {
                var deletedCommunityCategory = await _communityCategoryService.DeleteCommunityCategoryAsync(communityCategoryId);

                if (deletedCommunityCategory == null)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
