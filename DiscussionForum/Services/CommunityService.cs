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
                IEnumerable<Community> _communities = _unitOfWork.Community.GetAll();
                //Checks if the communities is valid
                if (_communities == null)
                {
                    throw new Exception("Community not found");
                }
                List<CommunityDTO> _communitiesDTO = new();

                foreach (Community _community in _communities)
                {
                    int _categoryCount = 0;
                    //Retrieves the list of category IDs in the community
                    List<long> _communityCategories = _context.CommunityCategoryMapping
                        .Where(c => c.CommunityID == _community.CommunityID)
                        .Select(c => c.CommunityCategoryID)
                        .ToList();
                    _categoryCount = _communityCategories.Count();

                    Dictionary<long, int> _categoryIdThreadCountMapping = new();
                    int _postCount = 0;
                    foreach (long _categoryID in _communityCategories)
                    {
                        IEnumerable<Threads> _threads = _context.Threads
                            .Include(t => t.CommunityCategoryMapping)
                            .Where(t => t.CommunityCategoryMapping.CommunityCategoryID == _categoryID);

                        int _threadCount = _threads.Count();
                        //Maps the number of posts/threads in a category with the category ID
                        _categoryIdThreadCountMapping[_categoryID] = _threadCount;
                        //Counts the total number of posts/threads in the community
                        _postCount += _threadCount;
                    }

                    //Retrieves the community data along with count of total number of categories, posts/threads and names of top n categories
                    _communitiesDTO.AddRange(
                        await _context.Communities
                        .Include(c => c.CommunityStatus)
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.ModifiedByUser)
                        .Where(c => c.CommunityID == _community.CommunityID)
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
                            TopCategories = GetTopCategories(_categoryIdThreadCountMapping, 5)
                        })
                        .ToListAsync()
                        );
                }

                return _communitiesDTO;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new ApplicationException("Error occurred while retrieving all communities", ex);
            }

        }

        public async Task<CommunityDTO> GetCommunityByIdAsync(int communityId)
        {
            try
            {
                Community _community = _context.Communities.Find(communityId);
                //Checks if the community is valid
                if (_community == null)
                {
                    throw new Exception("Community not found");
                }

                int _categoryCount = 0;
                //Retrieves the list of category IDs in the community
                List<long> _communityCategories = _context.CommunityCategoryMapping
                    .Where(c => c.CommunityID == _community.CommunityID)
                    .Select(c => c.CommunityCategoryID)
                    .ToList();
                _categoryCount = _communityCategories.Count();

                Dictionary<long, int> _categoryIdThreadCountMapping = new();
                int _postCount = 0;
                foreach (long _categoryID in _communityCategories)
                {
                    IEnumerable<Threads> _threads = _context.Threads
                        .Include(t => t.CommunityCategoryMapping)
                        .Where(t => t.CommunityCategoryMapping.CommunityCategoryID == _categoryID);

                    int _threadCount = _threads.Count();
                    //Maps the number of posts/threads in a category with the category ID
                    _categoryIdThreadCountMapping[_categoryID] = _threadCount;
                    //Counts the total number of posts/threads in the community
                    _postCount += _threadCount;
                }

                //Retrieves the community data along with count of total number of categories, posts/threads and names of top n categories
                CommunityDTO _communityDTO =
                    await _context.Communities
                    .Include(c => c.CommunityStatus)
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.ModifiedByUser)
                    .Where(c => c.CommunityID == _community.CommunityID)
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
                        TopCategories = GetTopCategories(_categoryIdThreadCountMapping, 5)
                    }).FirstAsync();

                return _communityDTO;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new ApplicationException($"Error occurred while retrieving community with ID = {communityId}", ex);
            }
        }

        /// <summary>
        /// Helper function to retrieve top n categories
        /// </summary>
        /// <param name="categoryIdThreadCountMapping">Mapping of category ID with total number of posts/threads within the category</param>
        /// <param name="noOfCategories">The number of top categories to be returned</param>
        /// <returns>A list of the names of the top n categories</returns>
        private List<string> GetTopCategories(Dictionary<long, int> categoryIdThreadCountMapping, int noOfCategories)
        {
            List<string> _topCategories = new List<string>();
            //Sort and select the top n category IDs
            List<long> _topCategoryIDs = categoryIdThreadCountMapping
                .OrderByDescending(kv => kv.Value)
                .Take(noOfCategories)
                .Select(kv => kv.Key)
                .ToList();
            foreach (long _categoryID in _topCategoryIDs)
            {
                //Retrieves the top n category names
                _topCategories.AddRange(_context.CommunityCategories
                        .Where(c => c.CommunityCategoryID == _categoryID)
                        .Select(c => c.CommunityCategoryName)
                        .ToList()
                        );
            }
            return _topCategories;
        }
    }
}
