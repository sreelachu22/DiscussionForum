using DiscussionForum.Models.EntityModels;
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

        /// <summary>
        /// Searches for replies based on the entered search term in the "Content" column.
        /// </summary>
        /// <param name="searchTerm">The term to search for in reply content.</param>
        [HttpGet("SearchReplies")]
        public async Task<IActionResult> SearchReplies(string searchTerm)
        {
            try
            {
                if (string.IsNullOrEmpty(searchTerm))
                {
                    return BadRequest("Search term cannot be empty");
                }

                IEnumerable<Reply> sampleData = await _replyService.GetRepliesFromDatabaseAsync();

                var searchTermsArray = searchTerm.Split(' ');

                // Create a list to store the filtered replies
                var filteredReplies = new List<Reply>();

                foreach (var term in searchTermsArray)
                {
                    // Filtering based on a search term in content with ignore case 
                    var termFilteredData = sampleData
                        .Where(reply => reply.Content.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();

                    // Add the filtered replies to the result list
                    filteredReplies.AddRange(termFilteredData);
                }

                // Remove duplicate replies based on ReplyID
                var uniqueReplies = filteredReplies
                    .GroupBy(reply => reply.ReplyID)
                    .Select(group => group.First())
                    .ToList();

                // Return the filtered data.
                return Ok(uniqueReplies);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
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