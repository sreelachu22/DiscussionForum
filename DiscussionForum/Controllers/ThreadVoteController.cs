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
    public class ThreadVoteController : ControllerBase
    {
        private readonly IThreadVoteService _threadVoteService;

        public ThreadVoteController(IThreadVoteService threadVoteService)
        {
            _threadVoteService = threadVoteService;
        }

        [CustomAuth("User")]
        [HttpPost("vote")]
        /*public async Task<IActionResult> PostThreadVote(ThreadVoteDto threadVoteDto)
        {
            try
            {
                await _threadVoteService.CreateThreadVote(threadVoteDto);
                return Ok("Vote recorded successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }*/

        public async Task<IActionResult> PostThreadVote([FromBody] ThreadVoteDto threadVoteDto)
        {
            try
            {
                var resultDto = await _threadVoteService.CreateThreadVote(threadVoteDto);
                return Ok(resultDto);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

    }
}
