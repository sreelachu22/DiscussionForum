using DiscussionForum.Models.APIModels;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAngularDev")]

    public class TagController : ControllerBase
    {

        private readonly ITagService _tagService;

        public TagController(ITagService TagService)
        {
            _tagService = TagService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTags(Boolean isdel)
        {
            try
            {
                var tags = await _tagService.GetAllTagAsync(isdel);
                return Ok(tags);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in fetching Tags: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("Search")]

        public async Task<IActionResult> SearchTags(string keyword)
        {
            try
            {
                var tags = await _tagService.GeAllTagAsync(keyword);
                if (tags.Any())
                {
                    return Ok(tags);
                }
                else
                {
                    return NotFound("No tags found");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in fetching Searched Tags: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpPost] 
        public async Task<IActionResult> CreateTag([FromBody] string tagname,Guid createdby)
        {
            var notice = await _tagService.CreateTagAsync(tagname, createdby);
            return Ok(notice);
        }
    }
}
