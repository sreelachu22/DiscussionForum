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
            var threads = await _threadService.GetAllThreads(CommunityCategoryMappingID, pageNumber, pageSize);
            return Ok(threads);
        }
    }
}
