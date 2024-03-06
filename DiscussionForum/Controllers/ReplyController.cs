using DiscussionForum.Authorization;
using DiscussionForum.Models.APIModels;
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
        //[CustomAuth("User")]
        [HttpGet("SearchReplies")]
        public async Task<IActionResult> SearchReplies(string searchTerm)
        {
            try
            {
                if (string.IsNullOrEmpty(searchTerm))
                {
                    return BadRequest("Search term cannot be empty");
                }

                IEnumerable<Reply> _sampleData = await _replyService.GetAllRepliesAsync();

                var _searchTermsArray = searchTerm.Split(' ');

                // Create a list to store the filtered replies
                var _filteredReplies = new List<Reply>();

                foreach (var _term in _searchTermsArray)
                {
                    // Filtering based on a search term in content with ignore case 
                    var _termFilteredData = _sampleData
                        .Where(reply => reply.Content.IndexOf(_term, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();

                    // Add the filtered replies to the result list
                    _filteredReplies.AddRange(_termFilteredData);
                }

                // Remove duplicate replies based on ReplyID
                var _uniqueReplies = _filteredReplies
                    .GroupBy(reply => reply.ReplyID)
                    .Select(group => group.First())
                    .ToList();

                // Return the filtered data.
                return Ok(_uniqueReplies);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all replies.
        /// </summary>
       // [CustomAuth("Admin")]
        [HttpGet]
        public async Task<IActionResult> GetReplies()
        {
            try
            {
                IEnumerable<Reply> _replies = await _replyService.GetAllRepliesAsync();
                return Ok(_replies);
            }
            catch (Exception ex)
            {
                //Checks for an inner exception and returns corresponding error message
                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"Error while retrieving all replies \nError: {ex.InnerException.Message}");
                }
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Error while retrieving all replies \nError: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a reply based on the given reply ID.
        /// </summary>
        /// <param name="replyId">The ID of the reply to search for in replies.</param>
        //[CustomAuth("User")]
        [HttpGet("{replyId}")]
        public async Task<IActionResult> GetReplyById(long replyId)
        {
            try
            {
                //Validates the request data
                if (replyId <= 0)
                {
                    throw new Exception("Invalid replyId. It should be greater than zero.");
                }

                var _reply = await _replyService.GetReplyByIdAsync(replyId);

                //Checks if the retrieved reply is null
                if (_reply == null)
                {
                    throw new Exception($"Reply with ID {replyId} not found.");
                }

                return Ok(_reply);
            }
            catch (Exception ex)
            {
                //Checks for an inner exception and returns corresponding error message
                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"Error while retrieving reply with ID = {replyId} \nError: {ex.InnerException.Message}");
                }
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Error while retrieving reply with ID = {replyId} \nError: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a list of replies based on the given thread ID.
        /// </summary>
        /// <param name="threadId">The ID of the thread to search for in replies.</param>
        //[CustomAuth("Admin")]
        [HttpGet("ByThreadID/{threadId}")]
        public async Task<IActionResult> GetRepliesByThreadId(long threadId)
        {
            try
            {
                //Validates the request data
                if (threadId <= 0)
                {
                    throw new Exception("Invalid threadId. It should be greater than zero.");
                }

                IEnumerable<Reply> _replies = await _replyService.GetRepliesByThreadIdAsync(threadId);

                //Checks if the retrieved replies is/are null
                if (_replies == null)
                {
                    throw new Exception($"Replies from thread with ID {threadId} not found.");
                }

                return Ok(_replies);
            }
            catch (Exception ex)
            {
                //Checks for an inner exception and returns corresponding error message
                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"Error while retrieving replies from thread with ID = {threadId} \nError: {ex.InnerException.Message}");
                }
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Error while retrieving replies from thread with ID = {threadId} \nError: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a list of replies based on the given parent reply ID.
        /// </summary>
        /// <param name="parentReplyId">The ID of the parent reply to search for in replies.</param>
        /*[CustomAuth("Admin")]
        [HttpGet("ByParentReplyID/{parentReplyId}")]
        public async Task<IActionResult> GetRepliesByParentReplyId(long parentReplyId)
        {
            try
            {
                //Validates the request data
                if (parentReplyId <= 0)
                {
                    throw new Exception("Invalid parentReplyId. It should be greater than zero.");
                }

                IEnumerable<Reply> _replies = await _replyService.GetRepliesByParentReplyIdAsync(parentReplyId);

                //Checks if the retrieved replies is null
                if (_replies == null)
                {
                    throw new Exception($"Replies from reply with ID {parentReplyId} not found.");
                }

                return Ok(_replies);
            }
            catch (Exception ex)
            {
                //Checks for an inner exception and returns corresponding error message
                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"Error while retrieving replies from reply with ID = {parentReplyId} \nError: {ex.InnerException.Message}");
                }
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Error while retrieving replies from reply with ID = {parentReplyId} \nError: {ex.Message}");
            }
        }*/
        [HttpGet("GetRepliesByParentReplyId/{threadID}")]
        public async Task<ActionResult<IEnumerable<ReplyDTO>>> GetRepliesByParentReplyId(long threadID, long? parentReplyID = null)
        {
            try
            {
                var replies = await _replyService.GetRepliesByParentReplyIdAsync(threadID, parentReplyID);
                return Ok(replies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        /// <summary>
        /// Creates a new reply with content from request body.
        /// </summary>
        /// <param name="threadId">The ID of the thread to which reply is posted.</param>
        /// <param name="parentReplyId">The ID of the reply to which reply is posted. May be null if not applicable.</param>
        /// <param name="creatorId">The ID of the user posting the reply.</param>
        [CustomAuth("User")]
        [HttpPost("{threadId}")]
        public async Task<IActionResult> CreateReply(long threadId, Guid creatorId, [FromBody] string content, long? parentReplyId = null)
        {
            /*try
            {*/
                //Validates the request data
                if (threadId <= 0)
                {
                    throw new Exception("Invalid threadId. It should be greater than zero.");
                }
                else if (parentReplyId != null && parentReplyId <= 0)
                {
                    throw new Exception("Invalid parentReplyId. It should be greater than zero.");
                }
                else if (string.IsNullOrWhiteSpace(content))
                {
                    throw new Exception("Invalid content. It cannot be null or empty.");
                }
                else if (creatorId == Guid.Empty)
                {
                    throw new Exception("Invalid creatorId. It cannot be null or empty.");
                }

                Reply _reply = await _replyService.CreateReplyAsync(threadId, creatorId, content, parentReplyId);
                return Ok(_reply);
            /*}
            catch (Exception ex)
            {
                //Checks for an inner exception and returns corresponding error message
                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"Error while creating reply \nError: {ex.InnerException.Message}");
                }
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Error while creating reply \nError: {ex.Message}");
            }*/
        }

        /// <summary>
        /// Updates a reply with content from request body based on the given reply ID.
        /// </summary>
        /// <param name="replyId">The ID of the reply to be updated.</param>
        /// <param name="modifierId">The ID of the user editing the reply.</param>
        [CustomAuth("User")]
        [HttpPut("{replyId}")]
        public async Task<IActionResult> UpdateReply(long replyId, Guid modifierId, [FromBody] string content)
        {
            try
            {
                //Validates the request data
                if (replyId <= 0)
                {
                    throw new Exception("Invalid replyId. It should be greater than zero.");
                }
                else if (string.IsNullOrWhiteSpace(content))
                {
                    throw new Exception("Invalid content. It cannot be null or empty.");
                }
                else if (modifierId == Guid.Empty)
                {
                    throw new Exception("Invalid modifierId. It cannot be null or empty.");
                }

                Reply _reply = await _replyService.UpdateReplyAsync(replyId, modifierId, content);
                return Ok(_reply);
            }
            catch (Exception ex)
            {
                //Checks for an inner exception and returns corresponding error message
                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"Error while updating reply with ID = {replyId} \nError: {ex.InnerException.Message}");
                }
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Error while updating reply with ID = {replyId} \nError: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a reply based on the given reply ID.
        /// </summary>
        /// <param name="replyId">The ID of the reply to be deleted.</param>
        /// <param name="modifierId">The ID of the user deleting the reply.</param>
        [CustomAuth("User")]
        [HttpDelete("{replyId}")]
        public async Task<IActionResult> DeleteReply(long replyId, Guid modifierId)
        {
            try
            {
                //Validates the request data
                if (replyId <= 0)
                {
                    throw new Exception("Invalid replyId. It should be greater than zero.");
                }
                else if (modifierId == Guid.Empty)
                {
                    throw new Exception("Invalid modifierId. It cannot be null or empty.");
                }

                Reply _reply = await _replyService.DeleteReplyAsync(replyId, modifierId);
                return Ok(_reply);
            }
            catch (Exception ex)
            {
                //Checks for an inner exception and returns corresponding error message
                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"Error while deleting reply with ID = {replyId} \nError: {ex.InnerException.Message}");
                }
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Error while deleting reply with ID = {replyId} \nError: {ex.Message}");
            }
        }

        // Http Get Method to get all the replies of a post in a nested manner.
        // We can get all the replies from a specific parent by providing the parentReplyId.
        // ParentReplyId of the first reply is null
        //[CustomAuth("User")]
        [HttpGet("getAllNestedRepliesOfaPost/{threadId}")]
        public IActionResult GetAllRepliesOfAPost(long threadId, long? parentReplyId = null, int page = 1, int pageSize = 10)
        {
            if (parentReplyId.HasValue && parentReplyId < 1)
            {
                return BadRequest("Invalid parentReplyId. It must be a positive integer or null.");
            }

            try
            {
                var replies = _replyService.GetAllRepliesOfAPost(threadId, parentReplyId, page, pageSize);

                if (replies.Any())
                {
                    return Ok(replies);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [CustomAuth("User")]
        [HttpGet("unviewed")]
        public IActionResult GetUnviewedReplies(Guid userId, int? categoryId, string sortDirection, int pageNumber, int pageSize)
        {
            try
            {
                var (replies, totalCount) = _replyService.GetUnviewedReplies(userId, categoryId, sortDirection, pageNumber, pageSize);
                return Ok(new { replies, totalCount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [CustomAuth("User")]
        [HttpPost("{replyId}/updateHasViewed")]
        public async Task<IActionResult> UpdateHasViewed(long replyId)
        {
            var success = await _replyService.UpdateHasViewed(replyId);
            if (success)
            {
                return Ok();
            }

            return NotFound();
        }
/*
        [CustomAuth("User")]*/
        [HttpPost("updateAllHasViewed")]
        public async Task<IActionResult> UpdateAllHasViewed([FromBody] long[] replyIDs)
        {
            var success = await _replyService.UpdateAllHasViewedAsync(replyIDs);
            if (success)
            {
                return Ok();
            }

            return NotFound();
        }

    }
}