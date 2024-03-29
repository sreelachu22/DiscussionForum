﻿using DiscussionForum.Authorization;
using DiscussionForum.ExceptionFilter;
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
        [CustomAuth("User")]
        [HttpGet]
        public async Task<IActionResult> GetThreads(int CommunityCategoryMappingID, int pageNumber, int pageSize, int filterOption, int sortOption)
        {
            try
            {
                var result = await _threadService.GetAllThreads(CommunityCategoryMappingID, pageNumber, pageSize, filterOption, sortOption);

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

        [CustomAuth("User")]
        [HttpGet("top-threads")]
        public async Task<IActionResult> GetTopThreads(int CommunityCategoryMappingID, string sortBy, int topCount)
        {
            try
            {
                var threads = await _threadService.GetTopThreads(CommunityCategoryMappingID, sortBy, topCount);
                return Ok(threads);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while fetching top threads: {ex.Message}");
            }
        }

        [CustomAuth("Head")]
        [HttpGet("ClosedThreads")]
        public async Task<IActionResult> GetClosedThreads(int CommunityID, int pageNumber, int pageSize)
        {
            try
            {
                var result = await _threadService.GetClosedThreads(CommunityID, pageNumber, pageSize);

                var response = new
                {
                    Threads = result.Threads,
                    TotalCount = result.TotalCount,
                    CommunityName = result.CommunityName
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetClosedThreads: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// Retrieves a thread based on the given thread ID.
        /// </summary>
        /// <param name="threadId">The ID of the thread to search for in threads.</param>
        [CustomAuth("User")]
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

                CategoryThreadDto _thread = await _threadService.GetThreadByIdAsync(threadId);

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

        public struct ThreadContent
        {
            public string Title { get; set; }
            public string Content { get; set; }

            public List<string> Tags { get; set; }
        }
        /// <summary>
        /// Creates a new thread with content from request body.
        /// </summary>
        /// <param name="CommunityCategoryMappingId">he mapping ID of the category in a community where threads must be posted.</param>
        /// <param name="CreatorId">The ID of the user posting the thread.</param>
        [CustomAuth("User")]
        [HttpPost]
        public async Task<IActionResult> CreateThread(int communityMappingId, Guid userId, [FromBody] ThreadContent threadcontent)
        {
            try
            {
                //Validates the request data

                if (communityMappingId <= 0)
                {
                    throw new Exception("Invalid CommunityCategoryMappingId. It should be greater than zero.");
                }
                else if (string.IsNullOrWhiteSpace(threadcontent.Title))
                {
                    throw new Exception("Invalid title. It cannot be null or empty.");
                }
                else if (string.IsNullOrWhiteSpace(threadcontent.Content))
                {
                    throw new Exception("Invalid content. It cannot be null or empty.");
                }
                else if (threadcontent.Tags == null || threadcontent.Tags.Count == 0)
                {
                    throw new Exception("Invalid content. It cannot be null or empty.");
                }
                else if (userId == Guid.Empty)
                {
                    throw new Exception("Invalid creatorId. It cannot be null or empty.");
                }
                CategoryThreadDto categorythreaddto = new CategoryThreadDto(
                    title: threadcontent.Title,
                    content: threadcontent.Content,
                    tagnames: threadcontent.Tags
                );

                Threads _thread = await _threadService.CreateThreadAsync(categorythreaddto, communityMappingId, userId);
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
        [CustomAuth("User")]
        [HttpPut("{threadId}")]
        public async Task<IActionResult> UpdateThread(long threadId, Guid ModifierId, [FromBody] ThreadContent titleContent)
        {
            try
            {
                //Validates the request data
                if (threadId <= 0)
                {
                    throw new Exception("Invalid threadId. It should be greater than zero.");
                }
                else if (
                    (string.IsNullOrWhiteSpace(titleContent.Title) && string.IsNullOrWhiteSpace(titleContent.Content))
                    || (string.IsNullOrEmpty(titleContent.Title) && string.IsNullOrEmpty(titleContent.Content))
                    )
                {
                    throw new Exception("Invalid request body. Both title and content cannot be null or empty.");
                }
                else if (string.IsNullOrEmpty(titleContent.Title) || string.IsNullOrWhiteSpace(titleContent.Title))
                {
                    titleContent.Title = null;
                }
                else if (string.IsNullOrEmpty(titleContent.Content) || string.IsNullOrWhiteSpace(titleContent.Title))
                {
                    titleContent.Content = null;
                }
                else if (ModifierId == Guid.Empty)
                {
                    throw new Exception("Invalid modifierId. It cannot be null or empty.");
                }

                Threads _thread = await _threadService.UpdateThreadAsync(threadId, ModifierId, titleContent.Title, titleContent.Content);
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

        [CustomAuth("User")]
        [HttpPut("CloseThread/{threadId}")]
        public async Task<IActionResult> CloseThread(long threadId, Guid ModifierId)
        {
            try
            {
                if (threadId <= 0)
                {
                    throw new Exception("Invalid threadId. It should be greater than zero.");
                }
                else if (ModifierId == Guid.Empty)
                {
                    throw new Exception("Invalid modifierId. It cannot be null or empty.");
                }

                Threads _thread = await _threadService.CloseThreadAsync(threadId, ModifierId);
                return Ok(_thread);
            }
            catch (Exception ex)
            {
                //Checks for an inner exception and returns corresponding error message
                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"Error while closing thread with ID = {threadId} \nError: {ex.InnerException.Message}");
                }
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Error while closing thread with ID = {threadId} \nError: {ex.Message}");
            }
        }

        [CustomAuth("User")]
        [HttpPut("ReopenThread/{threadId}")]
        public async Task<IActionResult> ReopenThread(long threadId, Guid ModifierId)
        {
            try
            {
                if (threadId <= 0)
                {
                    throw new Exception("Invalid threadId. It should be greater than zero.");
                }
                else if (ModifierId == Guid.Empty)
                {
                    throw new Exception("Invalid modifierId. It cannot be null or empty.");
                }

                Threads _thread = await _threadService.ReopenThreadAsync(threadId, ModifierId);
                return Ok(_thread);
            }
            catch (Exception ex)
            {
                //Checks for an inner exception and returns corresponding error message
                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"Error while reopening thread with ID = {threadId} \nError: {ex.InnerException.Message}");
                }
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Error while reopening thread with ID = {threadId} \nError: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a thread based on the given thread ID.
        /// </summary>
        /// <param name="threadId">The ID of the thread to be deleted.</param>
        /// <param name="ModifierId">The ID of the user deleting the thread.</param>
        [CustomAuth("Admin")]
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
        [CustomAuth("User")]
        [HttpGet("SearchThreadsByTitle")]
        public async Task<IActionResult> SearchThreadsTitle(string searchTerm, int pageNumber, int pageSize)
        {
            try
            {

                if (string.IsNullOrEmpty(searchTerm))
                {
                    return BadRequest("Search term cannot be empty");
                }
                searchTerm = searchTerm.Trim();
                var (searchThreadDtoList, searchThreadDtoListLength, isSearchTag) = await _threadService.ThreadTitleSearch(searchTerm, pageNumber, pageSize);


                return Ok(new { SearchThreadDtoList = searchThreadDtoList, SearchThreadDtoListLength = searchThreadDtoListLength, IsSearchTag = isSearchTag });


            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [CustomAuth("User")]
        [HttpGet("SearchThreadsByTags")]
        public async Task<IActionResult> SearchThreadsTag(string searchTerm)
        {
            try
            {

                if (string.IsNullOrEmpty(searchTerm))
                {
                    return BadRequest("Search term cannot be empty");
                }
                searchTerm = searchTerm.Trim();
                var (searchTagList, isSearchTag) = await _threadService.ThreadTagSearch(searchTerm);

                return Ok(new { SearchTagList = searchTagList, IsSearchTag = isSearchTag });


            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [CustomAuth("User")]
        [HttpGet("displaySearchedThreads")]
        public async Task<IActionResult> DisplaySearchedThreads(string searchTerm, int pageNumber, int pageSize, int filterOption, int sortOption)
        {
            try
            {

                if (string.IsNullOrEmpty(searchTerm))
                {
                    return BadRequest("Search term cannot be empty");
                }
                searchTerm = searchTerm.Trim();
                var (threadDtoList, threadDtoListCount) = await _threadService.DisplaySearchedThreads(searchTerm, pageNumber, pageSize, filterOption, sortOption);




                return Ok(new { searchThreadDtoList = threadDtoList, searchThreadDtoListLength = threadDtoListCount });


            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [CustomAuth("User")]
        [HttpGet("MyThreads")]
        public async Task<IActionResult> GetMyThreads(Guid userId, int pageNumber, int pageSize, int filterOption, int sortOption)
        {
            try
            {
                var result = await _threadService.GetMyThreads(userId, pageNumber, pageSize, filterOption, sortOption);

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
                Console.WriteLine($"Exception in GetMyThreads: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [CustomAuth("User")]
        [HttpGet("CheckDuplicate/{threadId}")]
        public async Task<IActionResult> GetOriginalThreadId(long threadId)
        {
            if(threadId < 0) 
            {
                throw new CustomException(449, "Invalid thread ID");
            }

            long _originalThreadId = await _threadService.GetOriginalThreadIdAsync(threadId);
            return Ok(_originalThreadId);
        }

        [CustomAuth("User")]
        [HttpPost("MarkDuplicate/{duplicateThreadId}/{originalThreadId}")]
        public async Task<IActionResult> MarkDuplicateThread(long duplicateThreadId, long originalThreadId, Guid createdBy)
        {
            if (duplicateThreadId < 0 || originalThreadId < 0)
            {
                throw new CustomException(449, "Invalid thread ID");
            }
            else if (createdBy == Guid.Empty)
            {
                throw new CustomException(448, "Invalid creator ID");
            }

            DuplicateThreads _duplicateThread = await _threadService.MarkDuplicateThreadAsync(duplicateThreadId, originalThreadId, createdBy);
            if (_duplicateThread == null)
            {
                throw new CustomException(447, $"Could not mark thread with ID : {duplicateThreadId} as duplicate of thread with ID : {originalThreadId}");
            }
            return Ok(_duplicateThread);
        }

        [CustomAuth("User")]
        [HttpPut("EditDuplicate/{duplicateThreadId}/{originalThreadId}")]
        public async Task<IActionResult> EditDuplicateThread(long duplicateThreadId, long originalThreadId, Guid modifiedBy)
        {
            if (duplicateThreadId < 0 || originalThreadId < 0)
            {
                throw new CustomException(449, "Invalid thread ID");
            }
            else if (modifiedBy == Guid.Empty)
            {
                throw new CustomException(448, "Invalid modifier ID");
            }

            DuplicateThreads _duplicateThread = await _threadService.EditDuplicateThreadAsync(duplicateThreadId, originalThreadId, modifiedBy);
            if (_duplicateThread == null)
            {
                throw new CustomException(447, $"Could not edit thread with ID : {duplicateThreadId} as duplicate of thread with ID : {originalThreadId}");
            }
            return Ok(_duplicateThread);
        }

        [CustomAuth("User")]
        [HttpDelete("UnmarkDuplicate/{duplicateThreadId}")]
        public async Task<IActionResult> UnmarkDuplicateThread(long duplicateThreadId, Guid modifiedBy)
        {
            if (duplicateThreadId < 0)
            {
                throw new CustomException(449, "Invalid thread ID");
            }
            else if (modifiedBy == Guid.Empty)
            {
                throw new CustomException(448, "Invalid modifier ID");
            }

            DuplicateThreads _duplicateThread = await _threadService.UnmarkDuplicateThreadAsync(duplicateThreadId, modifiedBy);
            if (_duplicateThread == null)
            {
                throw new CustomException(447, $"Could not unmark thread with ID : {duplicateThreadId} as duplicate");
            }
            return Ok(_duplicateThread);
        }
    }
}
