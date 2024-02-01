using DiscussionForum.Models.EntityModels;
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


        /* get all threads related to a category*/
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

        [HttpGet("SearchThreads")]
        public async Task<IActionResult> SearchThread(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return BadRequest("Search term cannot be empty");
            }

            IEnumerable<Threads> sampleData = await _threadService.GetThreadsFromDatabaseAsync();

            // Split the search term into individual words
            var searchTermsArray = searchTerm.Split(' ');

            // Create a list to store the filtered threads
            var filteredThreads = new List<Threads>();

            // Iterate through each search term
            foreach (var term in searchTermsArray)
            {
                // Filtering based on a search term
                var termFilteredData = sampleData
                    .Where(thread => thread.Content.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                // Add the filtered threads to the result list
                filteredThreads.AddRange(termFilteredData);
            }

            // Remove duplicate threads based on threadID
            var uniqueThreads = filteredThreads
                .GroupBy(thread => thread.ThreadID)
                .Select(group => group.First())
                .ToList();

            // Further processing or returning the filtered data.
            return Ok(uniqueThreads);
        }

    }
}
