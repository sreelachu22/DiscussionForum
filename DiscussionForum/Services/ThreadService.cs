using DiscussionForum.Data;
using DiscussionForum.Services;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Sprache;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;


namespace DiscussionForum.Services
{
    public class ThreadService : IThreadService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        private readonly IPointService _pointService;
        private readonly ITagService _tagService;
        private readonly ICommunityCategoryMappingService _communityCategoryMappingService;

        public ThreadService(IUnitOfWork unitOfWork, AppDbContext context, IPointService pointService, ITagService tagService)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _pointService = pointService;
            _tagService = tagService;
        }


        /// <summary>
        /// GetAllThreads retrieves paginated threads for a specific community category mapping.
        ///It includes additional logic to obtain the total thread count, category name, and description.
        ///The method takes CommunityCategoryMappingID, pageNumber, and pageSize as inputs, returning
        ///a tuple with threads, total count, category name, and description. If an error occurs,
        ///a custom exception is thrown.
        /// </summary>
        /// <param name="CommunityCategoryMappingID"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<(IEnumerable<CategoryThreadDto> Threads, int TotalCount, string CategoryName, string CategoryDescription)> GetAllThreads(int CommunityCategoryMappingID, int pageNumber, int pageSize,int filterOption, int sortOption)
        {
            try
            {



                var categoryInfo = await _context.CommunityCategoryMapping
                .Where(ccm => ccm.CommunityCategoryMappingID == CommunityCategoryMappingID)
                .Select(ccm => new { CategoryName = ccm.CommunityCategory.CommunityCategoryName, CategoryDescription = ccm.Description })
                .FirstOrDefaultAsync();



                IEnumerable<CategoryThreadDto> threads = _context.Threads
                           .Include(t => t.CommunityCategoryMapping)
                           .ThenInclude(c => c.CommunityCategory)
                           .Include(t => t.ThreadStatus)
                           .Include(t => t.CreatedByUser)
                           .Include(t => t.ModifiedByUser)
                           .Include(t => t.ThreadVotes)
                           .Where(t => t.CommunityCategoryMapping.CommunityCategoryMappingID == CommunityCategoryMappingID && !t.IsDeleted)
                           .Select(t => new CategoryThreadDto
                           {
                               ThreadID = t.ThreadID,
                               Title = t.Title,
                               Content = t.Content,
                               CreatedBy = t.CreatedBy,
                               CreatedByUser = t.CreatedByUser.Name,
                               CreatedAt = (DateTime)t.CreatedAt,
                               ModifiedBy = t.ModifiedBy,
                               ModifiedByUser = t.ModifiedByUser.Name,
                               ModifiedAt = (DateTime)t.ModifiedAt,
                               ThreadStatusName = t.ThreadStatus.ThreadStatusName,
                               IsAnswered = t.IsAnswered,
                               UpVoteCount = t.ThreadVotes != null ? t.ThreadVotes.Count(tv => !tv.IsDeleted && tv.IsUpVote) : 0,
                               DownVoteCount = t.ThreadVotes != null ? t.ThreadVotes.Count(tv => !tv.IsDeleted && !tv.IsUpVote) : 0,
                               TagNames = (from ttm in _context.ThreadTagsMapping
                                           join tg in _context.Tags on ttm.TagID equals tg.TagID
                                           where ttm.ThreadID == t.ThreadID
                                           select tg.TagName).ToList(),
                               ReplyCount = _context.Replies.Count(r => r.ThreadID == t.ThreadID && !r.IsDeleted)
                           })
                           .ToList();


                // Apply sorting based on filterOption and sortOption
                switch (filterOption)
                {
                    case 0:
                        threads = sortOption == 1 ? threads.OrderBy(t => t.ReplyCount) : threads.OrderByDescending(t => t.ReplyCount);
                        break;
                    case 1:
                        threads = sortOption == 1 ? threads.OrderBy(t => t.UpVoteCount) : threads.OrderByDescending(t => t.UpVoteCount);
                        break;
                    case 2:
                        threads = sortOption == 1 ? threads.OrderBy(t => t.DownVoteCount) : threads.OrderByDescending(t => t.DownVoteCount);
                        break;
                    case 3:
                        threads = sortOption == 1 ? threads.OrderBy(t => t.CreatedAt) : threads.OrderByDescending(t => t.CreatedAt);
                        break;
                }

                var totalCount = threads.Count();



                threads = threads
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();


                return (threads, totalCount, categoryInfo?.CategoryName ?? string.Empty, categoryInfo?.CategoryDescription ?? string.Empty);

            }
            catch (Exception ex)
            {
                throw new Exception("Error  while fetching threads.", ex);
            }
        }

        public async Task<IEnumerable<CategoryThreadDto>> GetTopThreads(int CommunityCategoryMappingID, string sortBy, int topCount)
        {
            try
            {
                IQueryable<Threads> query = _context.Threads
                    .Include(t => t.CommunityCategoryMapping)
                    .ThenInclude(c => c.CommunityCategory)
                    .Include(t => t.ThreadStatus)
                    .Include(t => t.CreatedByUser)
                    .Include(t => t.ModifiedByUser)
                    .Include(t => t.ThreadVotes)
                    //.Where(t => t.CommunityCategoryMapping.CommunityCategoryMappingID == CommunityCategoryMappingID && !t.IsDeleted && t.ThreadStatusID == 2);
                    .Where(t => t.CommunityCategoryMapping.CommunityCategoryMappingID == CommunityCategoryMappingID && !t.IsDeleted);

                switch (sortBy.ToLower())
                {
                    case "vote":
                        query = query.OrderByDescending(t => t.ThreadVotes.Count(tv => !tv.IsDeleted && tv.IsUpVote));
                        break;
                    case "latest":
                        query = query.OrderByDescending(t => t.CreatedAt);
                        break;
                    default:
                        throw new ArgumentException("Invalid sortBy parameter. Only 'votes' or 'createdAt' are allowed.");
                }

                var threads = await query.Take(topCount)
                    .Select(t => new CategoryThreadDto
                    {
                        ThreadID = t.ThreadID,
                        Title = t.Title,
                        Content = t.Content,
                        CreatedBy = t.CreatedBy,
                        CreatedByUser = t.CreatedByUser.Name,
                        CreatedAt = t.CreatedAt,
                        ModifiedBy = t.ModifiedBy,
                        ModifiedByUser = t.ModifiedByUser.Name,
                        ModifiedAt = t.ModifiedAt,
                        ThreadStatusName = t.ThreadStatus.ThreadStatusName,
                        IsAnswered = t.IsAnswered,
                        UpVoteCount = t.ThreadVotes != null ? t.ThreadVotes.Count(tv => !tv.IsDeleted && tv.IsUpVote) : 0,
                        DownVoteCount = t.ThreadVotes != null ? t.ThreadVotes.Count(tv => !tv.IsDeleted && !tv.IsUpVote) : 0,
                        TagNames = (from ttm in _context.ThreadTagsMapping
                                    join tg in _context.Tags on ttm.TagID equals tg.TagID
                                    where ttm.ThreadID == t.ThreadID
                                    select tg.TagName).ToList()
                    })
                    .ToListAsync();

                return threads;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching top threads.", ex);
            }
        }

        public async Task<(IEnumerable<CategoryThreadDto> Threads, int TotalCount, string CommunityName)> GetClosedThreads(int CommunityID, int pageNumber, int pageSize)
        {
            try
            {

                var _query = _context.Threads
                .Include(t => t.CommunityCategoryMapping)
                .Where(t => t.CommunityCategoryMapping.CommunityID == CommunityID && t.ThreadStatusID == 1 && !t.IsDeleted);
                var _totalCount = await _query.CountAsync();



                var _communityName = await _context.CommunityCategoryMapping
                .Where(ccm => ccm.CommunityID == CommunityID)
                .Select(ccm => ccm.Community.CommunityName)
                .FirstOrDefaultAsync();

                var _threads = await _context.Threads
                .Include(t => t.CommunityCategoryMapping)
                .ThenInclude(c => c.CommunityCategory)
                .Include(t => t.ThreadStatus)
                .Include(t => t.CreatedByUser)
                .Include(t => t.ModifiedByUser)
                .Include(t => t.ThreadVotes)
                    .Where(t => t.CommunityCategoryMapping.CommunityID == CommunityID && t.ThreadStatusID == 1 && !t.IsDeleted)
                    .OrderByDescending(t => t.CreatedAt)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(t => new CategoryThreadDto
                    {
                        ThreadID = t.ThreadID,
                        Title = t.Title,
                        Content = t.Content,
                        CreatedBy = t.CreatedBy,
                        CreatedByUser = t.CreatedByUser.Name,
                        CreatedAt = (DateTime)t.CreatedAt,
                        ModifiedBy = t.ModifiedBy,
                        ModifiedByUser = t.ModifiedByUser.Name,
                        ModifiedAt = (DateTime)t.ModifiedAt,
                        ThreadStatusName = t.ThreadStatus.ThreadStatusName,
                        IsAnswered = t.IsAnswered,
                        UpVoteCount = t.ThreadVotes != null ? t.ThreadVotes.Count(tv => !tv.IsDeleted && tv.IsUpVote) : 0,
                        DownVoteCount = t.ThreadVotes != null ? t.ThreadVotes.Count(tv => !tv.IsDeleted && !tv.IsUpVote) : 0,
                        TagNames = (from ttm in _context.ThreadTagsMapping
                                    join tg in _context.Tags on ttm.TagID equals tg.TagID
                                    where ttm.ThreadID == t.ThreadID
                                    select tg.TagName).ToList()

                    })
                    .ToListAsync();


                return (_threads, _totalCount, _communityName ?? string.Empty);

            }
            catch (Exception ex)
            {
                throw new Exception("Error  while fetching threads.", ex);
            }
        }


        public async Task<CategoryThreadDto> GetThreadByIdAsync(long threadId)
        {
            try
            {
                var _thread = await _context.Threads
                .Include(t => t.CommunityCategoryMapping)
                .ThenInclude(c => c.CommunityCategory)
                .Include(t => t.ThreadStatus)
                .Include(t => t.CreatedByUser)
                .Include(t => t.ModifiedByUser)
                .Include(t => t.ThreadVotes)
                    //.Where(t => t.ThreadID == threadId && t.ThreadStatusID == 2)
                    .Where(t => t.ThreadID == threadId)
                    .Select(t => new CategoryThreadDto
                    {
                        ThreadID = t.ThreadID,
                        Title = t.Title,
                        Content = t.Content,
                        CreatedBy = t.CreatedBy,
                        CreatedByUser = t.CreatedByUser.Name,
                        CreatedAt = (DateTime)t.CreatedAt,
                        ModifiedBy = t.ModifiedBy,
                        ModifiedByUser = t.ModifiedByUser.Name,
                        ModifiedAt = (DateTime)t.ModifiedAt,
                        ThreadStatusName = t.ThreadStatus.ThreadStatusName,
                        IsAnswered = t.IsAnswered,
                        UpVoteCount = t.ThreadVotes != null ? t.ThreadVotes.Count(tv => !tv.IsDeleted && tv.IsUpVote) : 0,
                        DownVoteCount = t.ThreadVotes != null ? t.ThreadVotes.Count(tv => !tv.IsDeleted && !tv.IsUpVote) : 0,
                        TagNames = (from ttm in _context.ThreadTagsMapping
                                    join tg in _context.Tags on ttm.TagID equals tg.TagID
                                    where ttm.ThreadID == t.ThreadID
                                    select tg.TagName).ToList(),
                        ThreadOwnerEmail = t.CreatedByUser.Email,

                    })
                    .FirstOrDefaultAsync();

                return _thread;

            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while retrieving thread with ID {threadId}.", ex);
            }
        }


        /// <summary>
        /// The CreateThreadAsync method is an asynchronous function responsible for creating a new thread in the discussion forum.
        /// It performs validation checks for the existence of the community category and user, creates a new thread using the provided data, 
        /// handles tag creation and mapping, and triggers additional services for point calculation. Any exceptions during the process are 
        /// caught and rethrown with appropriate error messages.
        /// </summary>
        /// <param name="categorythreaddto"></param>
        /// <param name="communityCategoryId"></param>
        /// <param name="createdby"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<Threads> CreateThreadAsync(CategoryThreadDto categorythreaddto, int communityCategoryId, Guid createdby)
        {
            try
            {

                bool communityCategoryMappingExists = _context.CommunityCategoryMapping
                                            .Any(mapping => !mapping.IsDeleted);

                if (!communityCategoryMappingExists)
                {
                    throw new Exception("Category does not exists in your community");
                }

                User userexists = await Task.FromResult(_context.Users.Find(createdby));
                if (userexists == null)
                {
                    throw new Exception("User no valid or doesnt exists");
                }

                Threads newThread = CreateThread(categorythreaddto, communityCategoryId, createdby);

                foreach (string tagName in categorythreaddto.TagNames)
                {
                    string tagname = tagName.ToLower();
                    Tag tagexists = _context.Tags.SingleOrDefault(tag => tag.TagName.ToLower() == tagname);

                    if (tagexists == null)
                    {
                        Tag newTag = await _tagService.CreateTagAsync(tagname, createdby);
                        tagexists = newTag;
                    }

                    ThreadTagsMapping threadtagmapping = new ThreadTagsMapping
                    {
                        TagID = tagexists.TagID,
                        ThreadID = newThread.ThreadID,
                        IsDeleted = false,
                        CreatedBy = createdby,
                        CreatedAt = DateTime.Now,
                    };

                    _context.ThreadTagsMapping.Add(threadtagmapping);
                }
                await _context.SaveChangesAsync();

                await _pointService.ThreadCreated(createdby);

                return newThread;

            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while creating a thread.", ex);
            }
        }


        /// <summary> 
        ///The CreateThread method synchronously creates a new thread entity with specified details, adds it to the unit of work, 
        ///and returns the created thread.If an error occurs during the process, it throws an exception with an appropriate message.
        /// </summary>
        /// <param name="categorythreaddto"></param>
        /// <param name="communityCategoryId"></param>
        /// <param name="createdby"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        private Threads CreateThread(CategoryThreadDto categorythreaddto, int communityCategoryId, Guid createdby)
        {
            try
            {
                Threads thread = new Threads { CommunityCategoryMappingID = communityCategoryId, Title = categorythreaddto.Title, Content = categorythreaddto.Content, ThreadStatusID = 2, IsAnswered = false, IsDeleted = false, CreatedBy = createdby, CreatedAt = DateTime.Now };


                _unitOfWork.Threads.Add(thread);
                _unitOfWork.Complete();

                return thread;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while creating the thread.", ex);
            }
        }


        public async Task<Threads> UpdateThreadAsync(long threadId, Guid modifierId, string? title, string? content)
        {
            try
            {
                Threads _thread = await Task.FromResult(_context.Threads.Find(threadId));
                User _modifier = await Task.FromResult(_context.Users.Find(modifierId));
                //Checks if modifier is valid
                if (_modifier == null)
                {
                    throw new Exception("Modifier not found");
                }
                //Checks if thread is valid and not deleted
                else if (_thread != null && !_thread.IsDeleted)
                {
                    if (title == null && content != null)
                    {
                        _thread.Content = content;
                        _thread.ModifiedBy = modifierId;
                        _thread.ModifiedAt = DateTime.Now;

                        await _pointService.ThreadUpdated(modifierId);

                        _context.SaveChanges();

                        return _thread;
                    }
                    else if (title != null && content == null)
                    {
                        _thread.Title = title;
                        _thread.ModifiedBy = modifierId;
                        _thread.ModifiedAt = DateTime.Now;

                        await _pointService.ThreadUpdated(modifierId);

                        _context.SaveChanges();

                        return _thread;
                    }
                    else
                    {
                        _thread.Title = title;
                        _thread.Content = content;
                        _thread.ModifiedBy = modifierId;
                        _thread.ModifiedAt = DateTime.Now;

                        await _pointService.ThreadUpdated(modifierId);

                        _context.SaveChanges();

                        return _thread;
                    }
                }
                //Checks if the thread is valid but deleted
                else if (_thread != null && _thread.IsDeleted)
                {
                    throw new Exception("Thread has been deleted.");
                }
                else
                {
                    throw new Exception("Thread not found.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while updating a thread with ID {threadId}.", ex);
            }
        }

        public async Task<Threads> CloseThreadAsync(long threadId, Guid modifierId)
        {
            try
            {
                Threads _thread = await Task.FromResult(_context.Threads.Find(threadId));
                User _modifier = await Task.FromResult(_context.Users.Find(modifierId));
                //Checks if modifier is valid
                if (_modifier == null)
                {
                    throw new Exception("Modifier not found");
                }
                //Checks if thread is valid and not deleted
                else if (_thread != null && !_thread.IsDeleted)
                {
                    if (_thread.ThreadStatusID == 1)
                    {
                        throw new Exception("Thread is already closed");
                    }

                    _thread.ThreadStatusID = 1;
                    _thread.ModifiedBy = modifierId;
                    _thread.ModifiedAt = DateTime.Now;

                    await _pointService.ThreadUpdated(modifierId);

                    _context.SaveChanges();

                    return _thread;

                }
                //Checks if the thread is valid but deleted
                else if (_thread != null && _thread.IsDeleted)
                {
                    throw new Exception("Thread has been deleted.");
                }
                else
                {
                    throw new Exception("Thread not found.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while closing a thread with ID {threadId}.", ex);
            }
        }

        public async Task<Threads> ReopenThreadAsync(long threadId, Guid modifierId)
        {
            try
            {
                Threads _thread = await Task.FromResult(_context.Threads.Find(threadId));
                User _modifier = await Task.FromResult(_context.Users.Find(modifierId));
                //Checks if modifier is valid
                if (_modifier == null)
                {
                    throw new Exception("Modifier not found");
                }
                //Checks if thread is valid and not deleted
                else if (_thread != null && !_thread.IsDeleted)
                {
                    if (_thread.ThreadStatusID == 2)
                    {
                        throw new Exception("Thread is already open");
                    }

                    _thread.ThreadStatusID = 2;
                    _thread.ModifiedBy = modifierId;
                    _thread.ModifiedAt = DateTime.Now;

                    await _pointService.ThreadUpdated(modifierId);

                    _context.SaveChanges();

                    return _thread;

                }
                //Checks if the thread is valid but deleted
                else if (_thread != null && _thread.IsDeleted)
                {
                    throw new Exception("Thread has been deleted.");
                }
                else
                {
                    throw new Exception("Thread not found.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while reopening a thread with ID {threadId}.", ex);
            }
        }

        public async Task<Threads> DeleteThreadAsync(long threadId, Guid modifierId)
        {
            try
            {
                Threads _thread = await Task.FromResult(_context.Threads.Find(threadId));
                User _modifier = await Task.FromResult(_context.Users.Find(modifierId));
                //Checks if modifier is valid
                if (_modifier == null)
                {
                    throw new Exception("Modifier not found");
                }
                //Checks if thread is valid and not deleted
                else if (_thread != null && !_thread.IsDeleted)
                {
                    _thread.IsDeleted = true;
                    _thread.ModifiedBy = modifierId;
                    _thread.ModifiedAt = DateTime.Now;

                    await _pointService.ThreadDeleted(modifierId);

                    _context.SaveChanges();

                    return _thread;
                }
                //Checks if the thread is valid but deleted
                else if (_thread != null && _thread.IsDeleted)
                {
                    throw new Exception("Thread has been deleted.");
                }
                else
                {
                    throw new Exception("Thread not found.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while deleting a thread with ID {threadId}.", ex);
            }
        }

        public async Task<(IEnumerable<TagDto> SearchTagList,bool isSearchTag)> ThreadTagSearch(string searchTerm)
        {
            try {
                searchTerm = searchTerm.Trim();
                System.String[] searchTermsArray = [];
                searchTermsArray = searchTerm.Split("#", StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < searchTermsArray.Length; i++)
                {
                    searchTermsArray[i] = new string(searchTermsArray[i].Where(c => !char.IsWhiteSpace(c)).ToArray());

                }

                var SearchTagList = _context.Tags
                    .Where(tag => searchTermsArray.Any(term => tag.TagName.Contains(term) && !tag.IsDeleted))
                    .Select(tag => new TagDto
                    {
                        TagId = tag.TagID,
                        TagName = tag.TagName,
                        TagCount = _context.ThreadTagsMapping
                                        .Count(tt => tt.TagID == tag.TagID && !tt.IsDeleted)
                    })
                    .GroupBy(tagDto => tagDto.TagId)
                    .Select(group => group.First())
                    .ToList();





                return (SearchTagList, true);


            }
            catch (Exception ex) {
                Console.WriteLine($"Error in GetThreadsFromDatabaseAsync: {ex.Message}");
                throw;
            }
        }





        /// <summary>
        /// The GetThreadsFromDatabaseAsync function retrieves and filters threads based on a search term, paginates the results, and returns a tuple containing 
        /// a list of categorized thread DTOs and the total count of matching threads. It leverages asynchronous queries and includes detailed information about
        /// each thread, such as creator details, vote counts, and tag names.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<CategoryThreadDto> SearchThreadDtoList, int SearchThreadDtoListLength, bool isSearchTag)> ThreadTitleSearch(string searchTerm, int pageNumber, int pageSize)
        {
            try
            {

               
                searchTerm = searchTerm.Trim();
                System.String[] searchTermsArray=[];
                var filteredThreads = new List<Threads>();
                var sortedThreads = new List<Threads>();
                var searchThreadDtoList = new List<CategoryThreadDto>();
                


                List<Threads> threads = await _context.Threads
                    .Include(t => t.CommunityCategoryMapping)
                        .ThenInclude(c => c.CommunityCategory)
                    .Include(t => t.ThreadStatus)
                    .Include(t => t.CreatedByUser)
                    .Include(t => t.ModifiedByUser)
                    .Include(t => t.ThreadVotes)
                    .Where(t => t.CommunityCategoryMapping.IsDeleted == false)
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync();

                                                                 
                searchTermsArray = searchTerm.Split(' ');

                sortedThreads = threads
                                .Select(thread => new
                                {
                                    Thread = thread,
                                    TotalHits = searchTermsArray.Count(term => thread.Title.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                                })
                                .Where(result => result.TotalHits > 0)
                                .GroupBy(result => result.Thread.ThreadID)
                                .Select(group => new
                                {
                                    ThreadID = group.Key,
                                    TotalHits = group.Sum(result => result.TotalHits),
                                    Thread = group.OrderByDescending(result => result.TotalHits).ThenByDescending(result => result.Thread.ThreadID).First().Thread
                                })
                                .OrderByDescending(thread => thread.TotalHits)
                                .ThenByDescending(thread => thread.Thread.ThreadID)
                                .Select(thread => thread.Thread)
                                .ToList();


                var uniqueThreads = sortedThreads
                       .GroupBy(thread => thread.ThreadID)
                       .Select(group => group.First())
                       .ToList();

                foreach (var thread in uniqueThreads)
                {
                    var searchThreadDto = new CategoryThreadDto
                    {
                        ThreadID = thread.ThreadID,
                        Title = thread.Title,
                        Content = thread.Content,

                        CreatedBy = thread.CreatedBy,
                        CreatedByUser = _context.Users
                                        .Where(u => u.UserID == thread.CreatedBy)
                                        .Select(u => u.Name)
                                        .FirstOrDefault(),
                        CreatedAt = (DateTime)thread.CreatedAt,

                        ModifiedBy = thread.ModifiedBy,
                        ModifiedByUser = thread.ModifiedByUser?.Name,
                        ModifiedAt = (DateTime?)thread.ModifiedAt,

                        ThreadStatusName = thread.ThreadStatus?.ThreadStatusName,
                        IsAnswered = thread.IsAnswered,
                        UpVoteCount = thread.ThreadVotes != null ? thread.ThreadVotes.Count(tv => !tv.IsDeleted && tv.IsUpVote) : 0,
                        DownVoteCount = thread.ThreadVotes != null ? thread.ThreadVotes.Count(tv => !tv.IsDeleted && !tv.IsUpVote) : 0,

                        TagNames = (from ttm in _context.ThreadTagsMapping
                                    join tg in _context.Tags on ttm.TagID equals tg.TagID
                                    where ttm.ThreadID == thread.ThreadID
                                    select tg.TagName).ToList(),
                        ReplyCount = _context.Replies.Count(r => r.ThreadID == thread.ThreadID && !r.IsDeleted)

                    };

                    searchThreadDtoList.Add(searchThreadDto);
                }

                return (searchThreadDtoList, sortedThreads.Count,false);


            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetThreadsFromDatabaseAsync: {ex.Message}");
                throw;
            }
        }


        public async Task<(IEnumerable<CategoryThreadDto> threadDtoList,int threadDtoListCount)> DisplaySearchedThreads(string searchTerm, int pageNumber, int pageSize, int filterOption, int sortOption)
        {
            try
            {
                searchTerm = searchTerm.Trim();
                IEnumerable<CategoryThreadDto> threads=new List<CategoryThreadDto>();
                int totalCount;
                bool isSearchTag;
                if (searchTerm[0] == '#')
                {
                    string[] searchTermsArray = searchTerm.Split("#", StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < searchTermsArray.Length; i++)
                    {
                        searchTermsArray[i] = new string(searchTermsArray[i].Where(c => !char.IsWhiteSpace(c)).ToArray());
                    }

                    threads = await (
                        from tag in _context.Tags
                        where searchTermsArray.Any(term => tag.TagName.Contains(term) && !tag.IsDeleted)
                        join threadTag in _context.ThreadTagsMapping
                        on tag.TagID equals threadTag.TagID
                        join thread in _context.Threads
                        on threadTag.ThreadID equals thread.ThreadID
                        where !thread.CommunityCategoryMapping.IsDeleted
                        orderby thread.CreatedAt descending
                        select new CategoryThreadDto
                        {
                            ThreadID = thread.ThreadID,
                            Title = thread.Title,
                            Content = thread.Content,
                            CreatedBy = thread.CreatedBy,
                            CreatedByUser = thread.CreatedByUser.Name,
                            CreatedAt = thread.CreatedAt,
                            ModifiedBy = thread.ModifiedBy,
                            ModifiedByUser = thread.ModifiedByUser.Name,
                            ModifiedAt = thread.ModifiedAt,
                            ThreadStatusName = thread.ThreadStatus.ThreadStatusName,
                            IsAnswered = thread.IsAnswered,
                            UpVoteCount = thread.ThreadVotes != null ? thread.ThreadVotes.Count(tv => !tv.IsDeleted && tv.IsUpVote) : 0,
                            DownVoteCount = thread.ThreadVotes != null ? thread.ThreadVotes.Count(tv => !tv.IsDeleted && !tv.IsUpVote) : 0,
                            ReplyCount = _context.Replies.Count(r => r.ThreadID == thread.ThreadID && !r.IsDeleted),
                            TagNames = _context.ThreadTagsMapping
                                            .Where(tt => tt.ThreadID == thread.ThreadID && !tt.IsDeleted)
                                            .Select(tt => tt.Tag.TagName)
                                            .ToList()
                        })
                        .ToListAsync();

                    totalCount = threads.Count();

                }
                else {

                    (threads, totalCount, isSearchTag) = await ThreadTitleSearch(searchTerm, pageNumber, pageSize);

                }

                switch (filterOption)
                {
                    case 0:
                        threads = sortOption == 1 ? threads.OrderBy(t => t.ReplyCount) : threads.OrderByDescending(t => t.ReplyCount);
                        break;
                    case 1:
                        threads = sortOption == 1 ? threads.OrderBy(t => t.UpVoteCount) : threads.OrderByDescending(t => t.UpVoteCount);
                        break;
                    case 2:
                        threads = sortOption == 1 ? threads.OrderBy(t => t.DownVoteCount) : threads.OrderByDescending(t => t.DownVoteCount);
                        break;
                    case 3:
                        threads = sortOption == 1 ? threads.OrderBy(t => t.CreatedAt) : threads.OrderByDescending(t => t.CreatedAt);
                        break;
                }


                threads = threads
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return (threads, totalCount);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DisplayThreadByTag: {ex.Message}");
                throw;
            }
        }

        public async Task<(IEnumerable<CategoryThreadDto> Threads, int TotalCount, string CategoryName, string CategoryDescription)> GetMyThreads(Guid UserID, int pageNumber, int pageSize, int filterOption, int sortOption)
        {
            try
            {
                IEnumerable<CategoryThreadDto> threads = _context.Threads
                    .Include(t => t.CommunityCategoryMapping)
                    .ThenInclude(c => c.CommunityCategory)
                    .Include(t => t.ThreadStatus)
                    .Include(t => t.CreatedByUser)
                    .Include(t => t.ModifiedByUser)
                    .Include(t => t.ThreadVotes)
                    .Where(t => t.CreatedBy == UserID && !t.IsDeleted)
                    .Select(t => new CategoryThreadDto
                    {
                        ThreadID = t.ThreadID,
                        Title = t.Title,
                        Content = t.Content,
                        CreatedBy = t.CreatedBy,
                        CreatedByUser = t.CreatedByUser.Name,
                        CreatedAt = (DateTime)t.CreatedAt,
                        ModifiedBy = t.ModifiedBy,
                        ModifiedByUser = t.ModifiedByUser.Name,
                        ModifiedAt = (DateTime)t.ModifiedAt,
                        ThreadStatusName = t.ThreadStatus.ThreadStatusName,
                        IsAnswered = t.IsAnswered,
                        UpVoteCount = t.ThreadVotes != null ? t.ThreadVotes.Count(tv => !tv.IsDeleted && tv.IsUpVote) : 0,
                        DownVoteCount = t.ThreadVotes != null ? t.ThreadVotes.Count(tv => !tv.IsDeleted && !tv.IsUpVote) : 0,
                        TagNames = (from ttm in _context.ThreadTagsMapping
                                    join tg in _context.Tags on ttm.TagID equals tg.TagID
                                    where ttm.ThreadID == t.ThreadID
                                    select tg.TagName).ToList(),
                        ReplyCount = _context.Replies.Count(r => r.ThreadID == t.ThreadID && !r.IsDeleted)
                    })
                    .ToList();

                // Apply sorting based on filterOption and sortOption
                switch (filterOption)
                {
                    case 0:
                        threads = sortOption == 1 ? threads.OrderBy(t => t.ReplyCount) : threads.OrderByDescending(t => t.ReplyCount);
                        break;
                    case 1:
                        threads = sortOption == 1 ? threads.OrderBy(t => t.UpVoteCount) : threads.OrderByDescending(t => t.UpVoteCount);
                        break;
                    case 2:
                        threads = sortOption == 1 ? threads.OrderBy(t => t.DownVoteCount) : threads.OrderByDescending(t => t.DownVoteCount);
                        break;
                    case 3:
                        threads = sortOption == 1 ? threads.OrderBy(t => t.CreatedAt) : threads.OrderByDescending(t => t.CreatedAt);
                        break;
                }

                var totalCount = threads.Count();

                threads = threads
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return (threads, totalCount, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching threads.", ex);
            }
        }

    }
}
