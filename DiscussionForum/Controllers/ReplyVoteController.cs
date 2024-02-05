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

        [HttpPost]
        public async Task<IActionResult> PostReplyVote(ReplyVoteDto replyVoteDto)
        {
            try
            {
                var result = await _replyVoteService.CreateReplyVote(replyVoteDto);
                return Ok(new { ReplyVoteID = result });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
