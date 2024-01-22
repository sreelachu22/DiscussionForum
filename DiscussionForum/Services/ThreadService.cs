using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<Threads>> GetAllThreads(int CommunityCategoryMappingID)
        {
            try
            {
                var threads = await _context.Threads
                    .Include(t => t.CommunityCategoryMapping)
                    .Where(t => t.CommunityCategoryMapping.CommunityCategoryMappingID == CommunityCategoryMappingID)
                    .ToListAsync();

                return threads;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching threads.", ex);
            }
        }
    }
}
