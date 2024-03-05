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
    public class ReplyVoteController : ControllerBase
    {
        private readonly IReplyVoteService _replyVoteService;

        public ReplyVoteController(IReplyVoteService replyVoteService)
        {
            _replyVoteService = replyVoteService;
        }

        [CustomAuth("User")]
        [HttpPost("vote")]
        public async Task<IActionResult> Vote([FromBody] ReplyVoteDto voteDto)
        {
            try
            {
                await _replyVoteService.VoteAsync(voteDto);
                return Ok("Vote recorded successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
