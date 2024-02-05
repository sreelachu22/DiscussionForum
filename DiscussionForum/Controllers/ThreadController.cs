﻿using DiscussionForum.Models.EntityModels;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;


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

        /// <summary>
        /// Retrieves all threads in a category.
        /// </summary>
        /// <param name="CommunityCategoryMappingID">The mapping ID of the category in a community whose threads must be fetched.</param>
        [HttpGet]
        public async Task<IActionResult> GetThreads(int CommunityCategoryMappingID, int pageNumber, int pageSize)
        {
            try
            {
                var result = await _threadService.GetAllThreads(CommunityCategoryMappingID, pageNumber, pageSize);

                var response = new
                {
                    Threads = result.Threads,
                    TotalCount = result.TotalCount,
                    CategoryName = result.CategoryName,
                    CategoryDescription = result.CategoryDescription
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetThreads: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// Retrieves a thread based on the given thread ID.
        /// </summary>
        /// <param name="threadId">The ID of the thread to search for in threads.</param>
        [HttpGet("{threadId}")]
        public async Task<IActionResult> GetThreadById(long threadId)
        {
            try
            {
                //Validates the request data
                if (threadId <= 0)
                {
                    throw new Exception("Invalid threadId. It should be greater than zero.");
                }

                Threads _thread = await _threadService.GetThreadByIdAsync(threadId);

                //Checks if the retrieved thread is null
                if (_thread == null)
                {
                    throw new Exception($"Thread with ID {threadId} not found.");
                }

                return Ok(_thread);
            }
            catch (Exception ex)
            {
                //Checks for an inner exception and returns corresponding error message
                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"Error while retrieving thread with ID = {threadId} \nError: {ex.InnerException.Message}");
                }
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Error while retrieving thread with ID = {threadId} \nError: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new thread with content from request body.
        /// </summary>
        /// <param name="CommunityCategoryMappingId">he mapping ID of the category in a community where threads must be posted.</param>
        /// <param name="CreatorId">The ID of the user posting the thread.</param>
        [HttpPost]
        public async Task<IActionResult> CreateThread(int CommunityCategoryMappingId, Guid CreatorId, [FromBody] string content)
        {
            try
            {
                //Validates the request data
                if (CommunityCategoryMappingId <= 0)
                {
                    throw new Exception("Invalid CommunityCategoryMappingId. It should be greater than zero.");
                }
                else if (string.IsNullOrWhiteSpace(content))
                {
                    throw new Exception("Invalid content. It cannot be null or empty.");
                }
                else if (CreatorId == Guid.Empty)
                {
                    throw new Exception("Invalid creatorId. It cannot be null or empty.");
                }

                Threads _thread = await _threadService.CreateThreadAsync(CommunityCategoryMappingId, CreatorId, content);
                return Ok(_thread);
            }
            catch (Exception ex)
            {
                //Checks for an inner exception and returns corresponding error message
                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"Error while creating thread. \nError: {ex.InnerException.Message}");
                }
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Error while creating thread. \nError: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates a thread with content from request body based on the given thread ID.
        /// </summary>
        /// <param name="threadId">The ID of the thread to be updated.</param>
        /// <param name="ModifierId">The ID of the user editing the thread.</param>
        [HttpPut("{threadId}")]
        public async Task<IActionResult> UpdateThread(long threadId, Guid ModifierId, [FromBody] string content)
        {
            try
            {
                //Validates the request data
                if (threadId <= 0)
                {
                    throw new Exception("Invalid threadId. It should be greater than zero.");
                }
                else if (string.IsNullOrWhiteSpace(content))
                {
                    throw new Exception("Invalid content. It cannot be null or empty.");
                }
                else if (ModifierId == Guid.Empty)
                {
                    throw new Exception("Invalid modifierId. It cannot be null or empty.");
                }

                Threads _thread = await _threadService.UpdateThreadAsync(threadId, ModifierId, content);
                return Ok(_thread);
            }
            catch (Exception ex)
            {
                //Checks for an inner exception and returns corresponding error message
                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"Error while updating thread with ID = {threadId} \nError: {ex.InnerException.Message}");
                }
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Error while updating thread with ID = {threadId} \nError: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a thread based on the given thread ID.
        /// </summary>
        /// <param name="threadId">The ID of the thread to be deleted.</param>
        /// <param name="ModifierId">The ID of the user deleting the thread.</param>
        [HttpDelete("{threadId}")]
        public async Task<IActionResult> DeleteThread(long threadId, Guid ModifierId)
        {
            try
            {
                //Validates the request data
                if (threadId <= 0)
                {
                    throw new Exception("Invalid threadId. It should be greater than zero.");
                }
                else if (ModifierId == Guid.Empty)
                {
                    throw new Exception("Invalid modifierId. It cannot be null or empty.");
                }

                Threads _thread = await _threadService.DeleteThreadAsync(threadId, ModifierId);
                return Ok(_thread);
            }
            catch (Exception ex)
            {
                //Checks for an inner exception and returns corresponding error message
                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"Error while deleting thread with ID = {threadId} \nError: {ex.InnerException.Message}");
                }
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Error while deleting thread with ID = {threadId} \nError: {ex.Message}");
            }
        }


        /// <summary>
        /// Searches for threads based on the entered search term in the "Content" column.
        /// </summary>
        /// <param name="searchTerm">The term to search for in reply content.</param>
        [HttpGet("SearchThreads")]
        public async Task<IActionResult> SearchThread(string searchTerm)
        {
            try
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

                // Return the filtered data.
                return Ok(uniqueThreads);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

    }
}
