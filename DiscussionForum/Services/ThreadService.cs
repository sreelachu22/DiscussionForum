using DiscussionForum.Data;
using DiscussionForum.Services;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.UnitOfWork;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading;
using System;
using System.Reflection.Metadata;

namespace DiscussionForum.Services
{
    public class ThreadService : IThreadService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        private readonly IPointService _pointService;
        private readonly ITagService _tagService;

        public ThreadService(IUnitOfWork unitOfWork, AppDbContext context, IPointService pointService, ITagService tagService)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _pointService = pointService;
            _tagService = tagService;
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
                        UpVoteCount = t.ThreadVotes != null ? t.ThreadVotes.Count(tv => !tv.IsDeleted && tv.IsUpVote) : 0,
                        DownVoteCount = t.ThreadVotes != null ? t.ThreadVotes.Count(tv => !tv.IsDeleted && !tv.IsUpVote) : 0,
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
                    .Where(t => t.CommunityCategoryMappingID == CommunityCategoryMappingID);

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
                        CreatedBy = t.CreatedByUser.Name,
                        CreatedAt = t.CreatedAt,
                        ModifiedBy = t.ModifiedByUser != null ? t.ModifiedByUser.Name : null,
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
                /* get total count based on query*/
                var _query = _context.Threads
                .Include(t => t.CommunityCategoryMapping)
                .Where(t => t.CommunityCategoryMapping.CommunityID == CommunityID && t.ThreadStatusID == 1);
                var _totalCount = await _query.CountAsync();

                /* to get category related info*/

                var _communityName = await _context.CommunityCategoryMapping
                .Where(ccm => ccm.CommunityID == CommunityID)
                .Select(ccm => ccm.Community.CommunityName)
                .FirstOrDefaultAsync();

                /* get threads with limit(pagination)*/
                var _threads = await _context.Threads
                .Include(t => t.CommunityCategoryMapping)
                .ThenInclude(c => c.CommunityCategory)
                .Include(t => t.ThreadStatus)
                .Include(t => t.CreatedByUser)
                .Include(t => t.ModifiedByUser)
                .Include(t => t.ThreadVotes)
                    .Where(t => t.CommunityCategoryMapping.CommunityID == CommunityID && t.ThreadStatusID == 1)
                    .OrderByDescending(t => t.CreatedAt)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(t => new CategoryThreadDto
                    {
                        ThreadID = t.ThreadID,
                        Title = t.Title,
                        Content = t.Content,
                        CreatedBy = t.CreatedByUser.Name,
                        CreatedAt = (DateTime)t.CreatedAt,
                        ModifiedBy = t.ModifiedByUser.Name,
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

        public async Task<Threads> CreateThreadAsync(CategoryThreadDto categorythreaddto, int communityCategoryId, Guid createdby)
        {
            try
            {

                bool communityCategoryMappingExists = await Task.FromResult(_context.CommunityCategoryMapping.Any(mapping => mapping.CommunityCategoryID == communityCategoryId));
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
