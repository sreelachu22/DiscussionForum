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

        [HttpGet("SearchThreads")]
        public async Task<IActionResult> SearchThread(string searchTerm)
        {
            IEnumerable<Threads> sampleData = await _threadService.GetThreadsFromDatabaseAsync();



            // filtering based on a search term:
            var filteredData = sampleData.Where(thread => thread.Content.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            // Further processing or returning the filtered data.
            return Ok(filteredData);
        }
    }
}
