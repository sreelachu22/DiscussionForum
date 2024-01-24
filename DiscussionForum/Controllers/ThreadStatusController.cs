using Microsoft.AspNetCore.Mvc;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;

namespace DiscussionForum.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        [EnableCors("AllowAngularDev")]
    public class ThreadStatusController : ControllerBase
        {
            private readonly IThreadStatusService _threadStatusService;

            public ThreadStatusController(IThreadStatusService ThreadStatusService)
            {
                _threadStatusService = ThreadStatusService;
            }

            [HttpGet]
            public async Task<IActionResult> GetThreadStatus()
            {
                var threadStatus = await _threadStatusService.GetThreadStatusAsync();
                return Ok(threadStatus);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetThreadStatusById(int id)
            {
                var ThreadStatus = await _threadStatusService.GetThreadStatusByIdAsync(id);
                if (ThreadStatus == null)
                {
                    return NotFound();
                }
                return Ok(ThreadStatus);
            }
        }
    }
