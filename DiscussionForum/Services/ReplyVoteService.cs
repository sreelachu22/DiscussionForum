using DiscussionForum.Data;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Services
{
    public class ReplyVoteService : IReplyVoteService
    {
        private readonly AppDbContext _dbContext;

        private readonly IPointService _pointService;

        public ReplyVoteService(AppDbContext dbContext, IPointService pointService)
        {
            _dbContext = dbContext;
            _pointService = pointService;
        }
        // Manages the creation and modification of reply votes in the database.
        // Input: ReplyVoteDto object representing user's vote on a reply.
        // Output: None.
        // Functionality:
        // - Checks if a vote by the same user on the same reply already exists in the database.
        // - Updates the existing vote if found, toggling IsDeleted if the new vote matches the existing one, otherwise updating IsUpVote and IsDeleted.        
        // - Handles point adjustments via _pointService based on the type of vote (upvote or downvote).        

        public async Task VoteAsync(ReplyVoteDto voteDto)
        {           
            var existingReplyVote = await _dbContext.ReplyVotes
                .FirstOrDefaultAsync(rv => rv.UserID == voteDto.UserID && rv.ReplyID == voteDto.ReplyID);

            if (existingReplyVote != null)
            {
                // Update the existing ReplyVote with the data from the DTO                
                if (existingReplyVote.IsUpVote == voteDto.IsUpVote)
                {
                    // If the existing vote and the incoming vote are the same, set IsDeleted to true
                    existingReplyVote.IsDeleted = !existingReplyVote.IsDeleted;

                    if (existingReplyVote.IsUpVote)
                    {
                        await _pointService.RemoveReplyUpvote(existingReplyVote.UserID, existingReplyVote.ReplyID);
                    }
                    else
                    {
                        await _pointService.RemoveReplyDownvote(existingReplyVote.UserID, existingReplyVote.ReplyID);
                    }
                }
                else
                {                    
                    existingReplyVote.IsUpVote = voteDto.IsUpVote;
                    existingReplyVote.IsDeleted = voteDto.IsDeleted;

                    if (existingReplyVote.IsUpVote)
                    {
                        await _pointService.ReplyUpvoted(existingReplyVote.UserID, existingReplyVote.ReplyID);
                    }
                    else
                    {
                        await _pointService.ReplyDownvoted(existingReplyVote.UserID, existingReplyVote.ReplyID);
                    }
                }
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

                if (newReplyVote.IsUpVote)
                {
                    await _pointService.ReplyUpvoted(newReplyVote.UserID, newReplyVote.ReplyID);
                }
                else
                {
                    await _pointService.ReplyDownvoted(newReplyVote.UserID, newReplyVote.ReplyID);
                }

                await _dbContext.SaveChangesAsync();
            }
        }

    }
}