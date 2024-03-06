using DiscussionForum.Authorization;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAngularDev")]
    public class SavedPostController: ControllerBase
    {
        private readonly ISavedPostService _savedPostService;

        public SavedPostController(ISavedPostService savedPostService)
        {
            _savedPostService = savedPostService;
        }
        [CustomAuth("User")]
        [HttpPost("save")]
        public async Task<IActionResult> SavePost(SavedPostDTO savedPostDto)
        {
            try
            {
                await _savedPostService.SavePost(savedPostDto);
                return Ok();
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [CustomAuth("User")]
        [HttpDelete("{userId}/{threadId}")]
        public async Task<IActionResult> DeleteSavedPost(Guid userId, int threadId)
        {
            try
            {
                await _savedPostService.DeleteSavedPost(userId, threadId);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [CustomAuth("User")]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetSavedPostsByUserId(Guid userId)
        {
            try
            {
                var savedPosts = await _savedPostService.GetSavedPostsByUserId(userId);
                return Ok(savedPosts);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
