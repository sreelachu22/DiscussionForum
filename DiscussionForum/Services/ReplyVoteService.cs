using DiscussionForum.Data;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Services
{
    public class ReplyVoteService : IReplyVoteService
    {
        private readonly AppDbContext _context;

        public ReplyVoteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateReplyVote(ReplyVoteDto replyVoteDto)
        {
            try
            {
                var replyVote = new ReplyVote
                {
                    UserID = replyVoteDto.UserID,
                    ReplyID = replyVoteDto.ReplyID,
                    IsUpVote = replyVoteDto.IsUpVote,
                    CreatedBy = replyVoteDto.UserID, // Assuming CreatedBy is the same as UserID for simplicity
                    CreatedAt = DateTime.Now,
                    ModifiedBy = replyVoteDto.UserID,
                    ModifiedAt = DateTime.Now
                };

                _context.ReplyVotes.Add(replyVote);
                await _context.SaveChangesAsync();

                return replyVote.ReplyVoteID;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred during ReplyVote: {ex.Message}", ex);
            }
        }
    }
}
