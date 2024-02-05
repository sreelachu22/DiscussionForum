using DiscussionForum.Data;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Services
{
    public class ThreadVoteService : IThreadVoteService
    {
        private readonly AppDbContext _context;

        public ThreadVoteService(AppDbContext Context)
        {
            _context = Context;
        }

        public async Task<int> CreateThreadVote(ThreadVoteDto threadVoteDto)
        {
            try
            {
                var threadVote = new ThreadVote
                {
                    UserID = threadVoteDto.UserID,
                    ThreadID = threadVoteDto.ThreadID,
                    IsUpVote = threadVoteDto.IsUpVote,
                    CreatedBy = threadVoteDto.UserID, // Assuming CreatedBy is the same as UserID for simplicity
                    CreatedAt = DateTime.Now,
                    ModifiedBy = threadVoteDto.UserID,
                    ModifiedAt = DateTime.Now
            };

                _context.ThreadVotes.Add(threadVote);
                await _context.SaveChangesAsync();

                return threadVote.ThreadVoteID;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occured during Upvote: {ex.Message}", ex);
            }
        }
    }
}
