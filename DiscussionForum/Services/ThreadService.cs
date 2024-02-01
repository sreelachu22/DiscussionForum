﻿using DiscussionForum.Data;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.UnitOfWork;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading;
using System;

namespace DiscussionForum.Services
{
    public class ThreadService : IThreadService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public ThreadService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        
        /*GetAllThreads retrieves paginated threads for a specific community category mapping.
        It includes additional logic to obtain the total thread count, category name, and description.
        The method takes CommunityCategoryMappingID, pageNumber, and pageSize as inputs, returning 
        a tuple with threads, total count, category name, and description. If an error occurs,
        a custom exception is thrown.*/
        public async Task<(IEnumerable<CategoryThreadDto> Threads, int TotalCount, string CategoryName, string CategoryDescription)> GetAllThreads(int CommunityCategoryMappingID, int pageNumber, int pageSize)
        {
            try
            {
                /* get total count based on query*/
                var query = _context.Threads
                .Include(t => t.CommunityCategoryMapping)
                .Where(t => t.CommunityCategoryMapping.CommunityCategoryMappingID == CommunityCategoryMappingID);
                var totalCount = await query.CountAsync();

                /* to get category related info*/

                var categoryInfo = await _context.CommunityCategoryMapping
                .Where(ccm => ccm.CommunityCategoryMappingID == CommunityCategoryMappingID)
                .Select(ccm => new { CategoryName = ccm.CommunityCategory.CommunityCategoryName, CategoryDescription = ccm.Description })
                .FirstOrDefaultAsync();

                /* get threads with limit(pagination)*/
                var threads = await _context.Threads
                .Include(t => t.CommunityCategoryMapping)
                .ThenInclude(c => c.CommunityCategory)
                .Include(t => t.ThreadStatus)
                .Include(t => t.CreatedByUser)
                .Include(t => t.ModifiedByUser)
                .Include(t => t.ThreadVotes)
                    .Where(t => t.CommunityCategoryMapping.CommunityCategoryMappingID == CommunityCategoryMappingID)
                    .OrderByDescending(t => t.CreatedAt)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(t => new CategoryThreadDto
                    {
                        ThreadID = t.ThreadID,
                        Content = t.Content,
                        CreatedBy = t.CreatedByUser.Name,
                        CreatedAt = (DateTime)t.CreatedAt,
                        ModifiedBy = t.ModifiedByUser.Name,
                        ModifiedAt = (DateTime)t.ModifiedAt,
                        ThreadStatusName = t.ThreadStatus.ThreadStatusName,
                        IsAnswered = t.IsAnswered,
                        VoteCount = t.ThreadVotes != null ? t.ThreadVotes.Count(tv => !tv.IsDeleted && tv.IsUpVote) : 0,
                        TagNames = (from ttm in _context.ThreadTagsMapping
                                    join tg in _context.Tags on ttm.TagID equals tg.TagID
                                    where ttm.ThreadID == t.ThreadID
                                    select tg.TagName).ToList()

            })
                    .ToListAsync();


                return (threads, totalCount, categoryInfo?.CategoryName ?? string.Empty, categoryInfo?.CategoryDescription ?? string.Empty);

            }
            catch (Exception ex)
            {
                throw new Exception("Error  while fetching threads.", ex);
            }
        }

        public async Task<Threads> GetThreadByIdAsync(long threadId)
        {
            try
            {
                return await Task.FromResult(_context.Threads.Find(threadId));

            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while retrieving thread with ID {threadId}.", ex);
            }
        }

        public async Task<Threads> CreateThreadAsync(int communityCategoryMappingId, Guid creatorId, string content)
        {
            try
            {
                return await Task.FromResult(CreateThread(communityCategoryMappingId, creatorId, content));
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                throw new ApplicationException($"Error occurred while creating a thread.", ex);
            }
        }

        private Threads CreateThread(int communityCategoryMappingId, Guid creatorId, string content)
        {
            Threads thread = new Threads { CommunityCategoryMappingID = communityCategoryMappingId, Content = content, ThreadStatusID = 2, IsAnswered = false, IsDeleted = false , CreatedBy = creatorId, CreatedAt = DateTime.Now};
            _unitOfWork.Threads.Add(thread);
            _unitOfWork.Complete();
            return thread;
        }

        public async Task<Threads> UpdateThreadAsync(long threadId, Guid modifierId, string content)
        {
            try
            {
                return await Task.FromResult(UpdateThread(threadId, modifierId, content));
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                throw new ApplicationException($"Error occurred while updating a thread with ID {threadId}.", ex);
            }
        }
        private Threads UpdateThread(long threadId, Guid modifierId, string content)
        {
            var thread = _context.Threads.Find(threadId);

            if (thread != null)
            {
                thread.Content = content;
                thread.ModifiedBy = modifierId;
                thread.ModifiedAt = DateTime.Now;
                _context.SaveChanges();
            }
            return thread;
        }

        public async Task DeleteThreadAsync(long threadId, Guid modifierId)
        {
            try
            {
                await Task.Run(() => DeleteThread(threadId, modifierId));
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                throw new ApplicationException($"Error occurred while deleting thread with ID {threadId}.", ex);
            }
        }
        private void DeleteThread(long threadId, Guid modifierId)
        {
            var thread = _context.Threads.Find(threadId);

            if (thread != null)
            {
                thread.IsDeleted = true;
                thread.ModifiedBy = modifierId;
                thread.ModifiedAt = DateTime.Now;
                _context.SaveChanges();
            }
        }

        //fetch threads from database

        public async Task<IEnumerable<Threads>> GetThreadsFromDatabaseAsync()
        {
            try
            {
                return await _context.Threads.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetThreadsFromDatabaseAsync: {ex.Message}");
                throw;
            }
        }

    }
}
