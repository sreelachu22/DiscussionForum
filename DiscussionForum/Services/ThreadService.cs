using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Models.APIModels;
using DiscussionForum.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Threading;

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

        public async Task<(IEnumerable<CategoryThreadDto> Threads, int TotalCount, string CategoryName, string CategoryDescription)> GetAllThreads(int CommunityCategoryMappingID, int pageNumber, int pageSize)
        {
            try
            {
                var query = _context.Threads
                    .Include(t => t.CommunityCategoryMapping)
                    .Where(t => t.CommunityCategoryMapping.CommunityCategoryMappingID == CommunityCategoryMappingID);

                var totalCount = await query.CountAsync();

                var categoryInfo = await _context.CommunityCategoryMapping
                    .Where(ccm => ccm.CommunityCategoryMappingID == CommunityCategoryMappingID)
                    .Select(ccm => new { CategoryName = ccm.CommunityCategory.CommunityCategoryName, CategoryDescription = ccm.Description })
                    .FirstOrDefaultAsync();


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
                        CreatedAt = t.CreatedAt,
                        ModifiedBy = t.ModifiedByUser.Name,
                        ModifiedAt = t.ModifiedAt,
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


        public async Task<IEnumerable<Threads>> GetThreadsFromDatabaseAsync()
        {
            try
            {
                var d = _context.Threads.ToList();
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
