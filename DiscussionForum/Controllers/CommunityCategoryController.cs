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

        // Get all categories in the discussion forum (created by super admin.
        // Community head will use these categories inside the community for community-category mapping.

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

        // GetByID

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

        // Post

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

        // Put

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
