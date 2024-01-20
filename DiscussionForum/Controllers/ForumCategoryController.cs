using DiscussionForum.Models.EntityModels;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DiscussionForum.Controllers
{
    [ApiController]
    [EnableCors("AllowAngularDev")]
    [Route("api/[controller]")]
    public class ForumCategoryController : ControllerBase
    {
        private readonly IForumCategoryService _forumCategoryService;

        public ForumCategoryController(IForumCategoryService forumCategoryService)
        {
            _forumCategoryService = forumCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetForumCategories()
        {
            var forumCategories = await _forumCategoryService.GetForumCategoriesAsync();
            return Ok(forumCategories);
        }

        [HttpGet("{forumCategoryId}")]
        public async Task<IActionResult> GetForumCategoryById(long forumCategoryId)
        {
            var forumCategory = await _forumCategoryService.GetForumCategoryByIdAsync(forumCategoryId);

            if (forumCategory == null)
                return NotFound();

            return Ok(forumCategory);
        }

        [HttpPost("{forumCategoryName}")]
        public async Task<IActionResult> CreateForumCategoryu(string forumCategoryName)
        {
            var forumCategory = await _forumCategoryService.CreateForumCategoryAsync(forumCategoryName);
            return Ok(forumCategory);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateForumCategory(long id, [FromBody] ForumCategory forumCategoryDto)
        {
            _forumCategoryService.UpdateForumCategoryAsync(id, forumCategoryDto);
            return NoContent();
        }

        [HttpDelete("{forumCategoryId}")]
        public async Task<IActionResult> DeleteForumCategory(long forumCategoryId)
        {
            await _forumCategoryService.DeleteForumCategoryAsync(forumCategoryId);
            return NoContent();
        }
    }
}
