using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
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

        /*[HttpPost]
        public async Task<ActionResult<int>> CreateCommunityCategoryMappingAsync(CommunityCategoryMappingAPI model)
        {
            try
            {
                var communityCategoryMappingID = await _communityCategoryMappingService.CreateCommunityCategoryMappingAsync(model);
                return Ok(communityCategoryMappingID);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }*/

        [HttpGet("ById/{communityCategoryMappingID}")] // Specify a unique route for GetCommunityCategoryMappingByIdAsync
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

        [HttpGet("InCommunity/{communityID}")] // Specify a unique route for GetAllCategoriesInCommunityAsync
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
                // Log the exception or handle it based on your application's needs
                return StatusCode(500, "Internal Server Error");
            }
        }



        [HttpPost("CreateWithCategoryName/{communityID}/{communityCategoryName}")]
        public async Task<ActionResult<int>> CreateCommunityCategoryMappingAsync(int communityID,[FromBody] string communityCategoryName, string description)
        {
            try
            {
                var communityCategoryMappingID = await _communityCategoryMappingService.CreateCommunityCategoryMappingAsync(communityID, communityCategoryName, description);
                return Ok(communityCategoryMappingID);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("{communityCategoryMappingID}")]
        public async Task<ActionResult<CommunityCategoryMapping>> UpdateCommunityCategoryMappingAsync(int communityCategoryMappingID, string description)
        {
            try
            {
                var result = await _communityCategoryMappingService.UpdateCommunityCategoryMappingAsync(communityCategoryMappingID, description);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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
