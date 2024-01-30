using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAngularDev")]
    public class ThreadController : ControllerBase
    {
        private readonly IThreadService _threadService;

        public ThreadController(IThreadService threadService)
        {
            _threadService = threadService;
        }

        [HttpGet]
        public async Task<IActionResult> GetThreads(int CommunityCategoryMappingID, int pageNumber, int pageSize)
        {
            var result = await _threadService.GetAllThreads(CommunityCategoryMappingID, pageNumber, pageSize);

            var response = new
            {
                Threads = result.Threads,
                TotalCount = result.TotalCount,
                CategoryName=result.CategoryName,
                CategoryDescription=result.CategoryDescription
            };

            return Ok(response);
        }

        [HttpGet("{threadId}")]
        public async Task<IActionResult> GetThreadById(long threadId)
        {
            var thread = await _threadService.GetThreadByIdAsync(threadId);

            if (thread == null)
                return NotFound();

            return Ok(thread);
        }

        [HttpPost]
        public async Task<IActionResult> CreateThread(int CommunityCategoryMappingId, Guid CreatorId, [FromBody] string content)
        {
            var thread = await _threadService.CreateThreadAsync(CommunityCategoryMappingId, CreatorId, content);
            return Ok(thread);
        }

        [HttpPut("{threadId}")]
        public async Task<IActionResult> UpdateThread(long threadId, Guid ModifierId, [FromBody] string content)
        {
            var thread = await _threadService.UpdateThreadAsync(threadId, ModifierId, content);
            return Ok(thread);
        }

        [HttpDelete("{threadId}")]
        public async Task<IActionResult> DeleteThread(long threadId, Guid ModifierId)
        {
            await _threadService.DeleteThreadAsync(threadId, ModifierId);
            return Ok();
        }
    }
}
