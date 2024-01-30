using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAngularDev")]
    public class ReplyController : ControllerBase
    {
        private readonly IReplyService _replyService;

        public ReplyController(IReplyService replyService)
        {
            _replyService = replyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReplies()
        {
            var replies = await _replyService.GetAllRepliesAsync();
            return Ok(replies);
        }

        [HttpGet("{replyId}")]
        public async Task<IActionResult> GetReplyById(long replyId)
        {
            var reply = await _replyService.GetReplyByIdAsync(replyId);

            if (reply == null)
                return NotFound();

            return Ok(reply);
        }

        [HttpGet("ByThreadID/{threadId}")]
        public async Task<IActionResult> GetRepliesByThreadId(long threadId)
        {
            var replies = await _replyService.GetRepliesByThreadIdAsync(threadId);

            if (replies == null)
                return NotFound();

            return Ok(replies);
        }

        [HttpGet("ByParentReplyID/{parentReplyId}")]
        public async Task<IActionResult> GetRepliesByParentReplyId(long parentReplyId)
        {
            var replies = await _replyService.GetRepliesByParentReplyIdAsync(parentReplyId);

            if (replies == null)
                return NotFound();

            return Ok(replies);
        }

        [HttpPost("{threadId},{parentReplyId}")]
        public async Task<IActionResult> CreateReply(long threadId, long parentReplyId,[FromBody] string content)
        {
            var reply = await _replyService.CreateReplyAsync(threadId, parentReplyId, content);
            return Ok(reply);
        }

        [HttpPut("{replyId}")]
        public async Task<IActionResult> UpdateReply(long replyId,[FromBody] string content)
        {
            var reply = await _replyService.UpdateReplyAsync(replyId, content);
            return Ok(reply);
        }

        [HttpDelete("{replyId}")]
        public async Task<IActionResult> DeleteReply(long replyId)
        {
            await _replyService.DeleteReplyAsync(replyId);
            return NoContent();
        }
    }
}