using DiscussionForum.Models.EntityModels;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> GetCommunityCategories()
        {
            var communityCategories = await _communityCategoryService.GetCommunityCategoriesAsync();
            return Ok(communityCategories);
        }

        [HttpGet("{communityCategoryId}")]
        public async Task<IActionResult> GetCommunityCategoryById(long communityCategoryId)
        {
            var communityCategory = await _communityCategoryService.GetCommunityCategoryByIdAsync(communityCategoryId);

            if (communityCategory == null)
                return NotFound();

            return Ok(communityCategory);
        }

        [HttpPost("{communityCategoryName}")]
        public async Task<IActionResult> CreateCommunityCategory(string communityCategoryName)
        {
            var communityCategory = await _communityCategoryService.CreateCommunityCategoryAsync(communityCategoryName);
            return Ok(communityCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCommunityCategory(long id, [FromBody] CommunityCategory communityCategoryDto)
        {
            var updatedCommunityCategory = await _communityCategoryService.UpdateCommunityCategoryAsync(id, communityCategoryDto);

            if (updatedCommunityCategory == null)
                return NotFound();

            return Ok(updatedCommunityCategory);
        }

        [HttpDelete("{communityCategoryId}")]
        public async Task<IActionResult> DeleteCommunityCategory(long communityCategoryId)
        {
            var deletedCommunityCategory = await _communityCategoryService.DeleteCommunityCategoryAsync(communityCategoryId);

            if (deletedCommunityCategory == null)
                return NotFound();

            return NoContent();
        }
    }
}
