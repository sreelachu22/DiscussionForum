﻿using DiscussionForum.Data;
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

        public async Task<ThreadVoteDto> CreateThreadVote(ThreadVoteDto threadVoteDto)
        {
            try
            {
                var _existingThreadVote = await _context.ThreadVotes
                .FirstOrDefaultAsync(tv => tv.UserID == threadVoteDto.UserID && tv.ThreadID == threadVoteDto.ThreadID);


                int upvoteCount = 0;
                int downvoteCount = 0;

                if (_existingThreadVote != null)
                {
                    // Update the existing ThreadVote with the data from the DTO                
                    if (_existingThreadVote.IsUpVote == threadVoteDto.IsUpVote)
                    {
                        // If the existing vote and the incoming vote are the same, set IsDeleted to true
                        _existingThreadVote.IsDeleted = !_existingThreadVote.IsDeleted;

                        if (_existingThreadVote.IsUpVote)
                        {
                            await _pointService.RemoveThreadUpvote(_existingThreadVote.UserID, _existingThreadVote.ThreadID);
                        }
                        else
                        {
                            await _pointService.RemoveThreadDownvote(_existingThreadVote.UserID, _existingThreadVote.ThreadID);
                        }
                    }
                    else
                    {
                        _existingThreadVote.IsUpVote = threadVoteDto.IsUpVote;
                        _existingThreadVote.IsDeleted = threadVoteDto.IsDeleted;

                        if (_existingThreadVote.IsUpVote)
                        {
                            await _pointService.ThreadUpvoted(_existingThreadVote.UserID, _existingThreadVote.ThreadID);
                        }
                        else
                        {
                            await _pointService.ThreadDownvoted(_existingThreadVote.UserID, _existingThreadVote.ThreadID);
                        }
                    }
                    _existingThreadVote.ModifiedAt = DateTime.Now;

                    await _context.SaveChangesAsync();

                    upvoteCount = await _context.ThreadVotes.CountAsync(tv => tv.ThreadID == threadVoteDto.ThreadID && !tv.IsDeleted && tv.IsUpVote);
                    downvoteCount = await _context.ThreadVotes.CountAsync(tv => tv.ThreadID == threadVoteDto.ThreadID && !tv.IsDeleted && !tv.IsUpVote);
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

                    if (_newThreadVote.IsUpVote)
                    {
                        await _pointService.ThreadUpvoted(_newThreadVote.UserID, _newThreadVote.ThreadID);
                    }
                    else
                    {
                        await _pointService.ThreadDownvoted(_newThreadVote.UserID, _newThreadVote.ThreadID);
                    }

                    await _context.SaveChangesAsync();

                    // Calculate upvote and downvote counts
                    upvoteCount = await _context.ThreadVotes.CountAsync(tv => tv.ThreadID == threadVoteDto.ThreadID && !tv.IsDeleted && tv.IsUpVote);
                    downvoteCount = await _context.ThreadVotes.CountAsync(tv => tv.ThreadID == threadVoteDto.ThreadID && !tv.IsDeleted && !tv.IsUpVote);
                }
                var threadvoteDto = new ThreadVoteDto
                {
                    UserID = threadVoteDto.UserID,
                    ThreadID = threadVoteDto.ThreadID,
                    IsUpVote = threadVoteDto.IsUpVote,
                    IsDeleted = threadVoteDto.IsDeleted,
                    UpvoteCount = upvoteCount,
                    DownvoteCount = downvoteCount
                };

                return threadvoteDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while voting for thread: {ex.Message}", ex);
            }
        }
    }
}
