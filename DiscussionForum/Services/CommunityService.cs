using DiscussionForum.Data;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Services
{
    public class CommunityService : ICommunityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public CommunityService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IEnumerable<CommunityDTO>> GetAllCommunitiesAsync()
        {
            try
            {
                var communities = _unitOfWork.Community.GetAll();
                List<CommunityDTO> _communities = new List<CommunityDTO>();

                foreach (var community in communities)
                {
                    int _categoryCount = 0;
                    List<long> communityCategories = _context.CommunityCategoryMapping
                        .Where(c => c.CommunityID == community.CommunityID)
                        .Select(c => c.CommunityCategoryID)
                        .ToList();
                    _categoryCount = communityCategories.Count();

                    Dictionary<long, int> categoryIDwithThreadCount = new Dictionary<long, int>();
                    int _postCount = 0;
                    foreach (long categoryID in communityCategories)
                    {
                        var query = _context.Threads
                            .Include(t => t.CommunityCategoryMapping)
                            .Where(t => t.CommunityCategoryMapping.CommunityCategoryID == categoryID);

                        int threadCount = query.Count();
                        categoryIDwithThreadCount[categoryID] = threadCount;
                        _postCount += threadCount;
                    }

                    _communities.AddRange(
                        await _context.Communities
                        .Include(c => c.CommunityStatus)
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.ModifiedByUser)
                        .Where(c => c.CommunityID == community.CommunityID)
                        .Select(c => new CommunityDTO
                        {
                            CommunityID = c.CommunityID,
                            CommunityName = c.CommunityName,
                            CommunityStatusName = c.CommunityStatus.CommunityStatusName,
                            CreatedBy = c.CreatedByUser.Name,
                            CreatedAt = c.CreatedAt,
                            ModifiedBy = c.ModifiedByUser.Name,
                            ModifiedAt = c.ModifiedAt,
                            CategoryCount = _categoryCount,
                            PostCount = _postCount,
                            TopCategories = GetTopCategories(categoryIDwithThreadCount)
                        })
                        .ToListAsync()
                        );
                }

                return _communities;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new ApplicationException("Error occurred while fetching communities", ex);
            }

        }

        private List<string> GetTopCategories(Dictionary<long, int> threadCountByCategory)
        {
            List<string> _topCategories = new List<string>();
            var topCategoryIDs = threadCountByCategory
                .OrderByDescending(kv => kv.Value)
                .Take(5)
                .Select(kv => kv.Key)
                .ToList();
            Console.WriteLine(topCategoryIDs);
            foreach (var categoryID in topCategoryIDs)
            {
                _topCategories.AddRange(_context.CommunityCategories
                        .Where(c => c.CommunityCategoryID == categoryID)
                        .Select(c => c.CommunityCategoryName)
                        .ToList()
                        );
            }
            return _topCategories;
        }
    }
}
