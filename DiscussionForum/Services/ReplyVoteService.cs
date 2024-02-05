using DiscussionForum.Data;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Services
{
    public class ReplyVoteService : IReplyVoteService
    {
        private readonly AppDbContext _dbContext;

        public ReplyVoteService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task VoteAsync(ReplyVoteDto voteDto)
        {           
            var existingReplyVote = await _dbContext.ReplyVotes
                .FirstOrDefaultAsync(rv => rv.UserID == voteDto.UserID && rv.ReplyID == voteDto.ReplyID);

            if (existingReplyVote != null)
            {
                // Update the existing ReplyVote with the data from the DTO
                existingReplyVote.IsUpVote = voteDto.IsUpVote;
                existingReplyVote.IsDeleted = voteDto.IsDeleted;
                existingReplyVote.ModifiedAt = DateTime.Now;                

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                // Create a new ReplyVote
                var newReplyVote = new ReplyVote
                {
                    UserID = voteDto.UserID,
                    ReplyID = voteDto.ReplyID,
                    IsUpVote = voteDto.IsUpVote,
                    IsDeleted = voteDto.IsDeleted,
                    CreatedBy = voteDto.UserID,
                    CreatedAt = DateTime.Now,                    
                };

                _dbContext.ReplyVotes.Add(newReplyVote);
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}