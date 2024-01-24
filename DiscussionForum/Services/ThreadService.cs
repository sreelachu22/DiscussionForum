using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Models.APIModels;
using DiscussionForum.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DiscussionForum.Models.APIModels;

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

        public async Task<IEnumerable<CategoryThreadDto>> GetAllThreads(int CommunityCategoryMappingID)
        {
            try
            {
                var threads = await _context.Threads
                    .Include(t => t.CommunityCategoryMapping)
                        .ThenInclude(c => c.CommunityCategory)
                    .Include(t => t.ThreadStatus)
                    .Include(t => t.CreatedByUser)
                    .Include(t => t.ModifiedByUser)
                    .Where(t => t.CommunityCategoryMapping.CommunityCategoryMappingID == CommunityCategoryMappingID)
                    .Select(t => new CategoryThreadDto
                    {
                        ThreadID = t.ThreadID,
                        Content = t.Content,
                        CreatedBy = t.CreatedByUser.Name, 
                        CreatedAt = t.CreatedAt,
                        ModifiedBy = t.ModifiedByUser.Name,
                        ModifiedAt = t.ModifiedAt,
                        CategoryName = t.CommunityCategoryMapping.CommunityCategory.CommunityCategoryName, 
                        ThreadStatusName = t.ThreadStatus.ThreadStatusName,
                        IsAnswered=t.IsAnswered
                    })
                    .ToListAsync();
                return threads;
            }
            catch (Exception ex)
            {
                throw new Exception("Error  while fetching threads", ex);
            }
        }
    }
}
