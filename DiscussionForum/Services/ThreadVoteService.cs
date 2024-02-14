using DiscussionForum.Data;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Services
{
    public class ThreadVoteService : IThreadVoteService
    {
        private readonly AppDbContext _context;

        private readonly IPointService _pointService;

        public ThreadVoteService(AppDbContext context, IPointService pointService)
        {
            _context = context;
            _pointService = pointService;
        }

        // Manages the creation and modification of thread votes in the database.
        // Input: ThreadVoteDto object representing user's vote on a thread.
        // Output: None.
        // Functionality: Handles thread voting operations, including upvoting and downvoting, while managing database transactions.

        public async Task CreateThreadVote(ThreadVoteDto threadVoteDto)
        {
            try
            {
                var _existingThreadVote = await _context.ThreadVotes
                .FirstOrDefaultAsync(tv => tv.UserID == threadVoteDto.UserID && tv.ThreadID == threadVoteDto.ThreadID);

                if (_existingThreadVote != null)
                {                    
                    _existingThreadVote.IsUpVote = threadVoteDto.IsUpVote;
                    _existingThreadVote.IsDeleted = threadVoteDto.IsDeleted;
                    _existingThreadVote.ModifiedAt = DateTime.Now;

                    if (_existingThreadVote.IsUpVote)
                    {
                        await _pointService.ThreadUpvoted(_existingThreadVote.UserID, _existingThreadVote.ThreadID);
                    }
                    else
                    {
                        await _pointService.ThreadDownvoted(_existingThreadVote.UserID, _existingThreadVote.ThreadID);
                    }
                    await _context.SaveChangesAsync();
                }
                else
                {                    
                    var _newThreadVote = new ThreadVote
                    {
                        UserID = threadVoteDto.UserID,
                        ThreadID = threadVoteDto.ThreadID,
                        IsUpVote = threadVoteDto.IsUpVote,
                        IsDeleted = threadVoteDto.IsDeleted,
                        CreatedBy = threadVoteDto.UserID,
                        CreatedAt = DateTime.Now,
                    };

                    _context.ThreadVotes.Add(_newThreadVote);

                    if (threadVoteDto.IsUpVote)
                    {
                        await _pointService.ThreadUpvoted(_newThreadVote.UserID, _newThreadVote.ThreadID);
                    }
                    else
                    {
                        await _pointService.ThreadDownvoted(_newThreadVote.UserID, _newThreadVote.ThreadID);
                    }

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while voting for thread: {ex.Message}", ex);
            }
        }
    }
}
