using DiscussionForum.Data;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;

namespace DiscussionForum.Services
{
    public class PointService : IPointService
    {
        private readonly AppDbContext _context;

        public PointService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<PointDto> GetPointsByCommunityId(int communityID)
        {
            var point = await _context.Points.FirstOrDefaultAsync(p => p.CommunityID == communityID);
            if (point == null)
            {
                throw new Exception("No points found for the specified community.");
            }

            return new PointDto
            {
                // Map properties from the retrieved point entity to PointDto
                ThreadCreated = point.ThreadCreated,
                ThreadUpdated = point.ThreadUpdated,
                ThreadDeleted = point.ThreadDeleted,
                ThreadUpvotedBy = point.ThreadUpvotedBy,
                ThreadUpvotedOn = point.ThreadUpvotedOn,
                ThreadDownvotedBy = point.ThreadDownvotedBy,
                ThreadDownvotedOn = point.ThreadDownvotedOn,
                RemoveThreadUpvoteBy = point.RemoveThreadUpvoteBy,
                RemoveThreadUpvoteOn = point.RemoveThreadUpvoteOn,
                RemoveThreadDownvoteBy = point.RemoveThreadDownvoteBy,
                RemoveThreadDownvoteOn = point.RemoveThreadDownvoteOn,
                ReplyCreated = point.ReplyCreated,
                ReplyUpdated = point.ReplyUpdated,
                ReplyDeleted = point.ReplyDeleted,
                ReplyUpvotedBy = point.ReplyUpvotedBy,
                ReplyUpvotedOn = point.ReplyUpvotedOn,
                ReplyDownvotedBy = point.ReplyDownvotedBy,
                ReplyDownvotedOn = point.ReplyDownvotedOn,
                RemoveReplyUpvoteBy = point.RemoveReplyUpvoteBy,
                RemoveReplyUpvoteOn = point.RemoveReplyUpvoteOn,
                RemoveReplyDownvoteBy = point.RemoveReplyDownvoteBy,
                RemoveReplyDownvoteOn = point.RemoveReplyDownvoteOn,
        };
    }
        public async Task<PointDto> UpdatePoint(int communityID, PointDto pointDto)
        {
            var point = await _context.Points.FirstOrDefaultAsync(p => p.CommunityID == communityID);

            if (point == null)
            {
                return null;
            }

            point.ThreadCreated = pointDto.ThreadCreated;
            point.ThreadUpdated = pointDto.ThreadUpdated;
            point.ThreadDeleted = pointDto.ThreadDeleted;
            point.ThreadUpvotedBy = pointDto.ThreadUpvotedBy;
            point.ThreadUpvotedOn = pointDto.ThreadUpvotedOn;
            point.ThreadDownvotedBy = pointDto.ThreadDownvotedBy;
            point.ThreadDownvotedOn = pointDto.ThreadDownvotedOn;
            point.RemoveThreadUpvoteBy = pointDto.RemoveThreadUpvoteBy;
            point.RemoveThreadUpvoteOn = pointDto.RemoveThreadUpvoteOn;
            point.RemoveThreadDownvoteBy = pointDto.RemoveThreadDownvoteBy;
            point.RemoveThreadDownvoteOn = pointDto.RemoveThreadDownvoteOn;
            point.ReplyCreated = pointDto.ReplyCreated;
            point.ReplyUpdated  = pointDto.ReplyUpdated;
            point.ReplyDeleted = pointDto.ReplyDeleted;
            point.ReplyUpvotedBy = pointDto.ReplyUpvotedBy;
            point.ReplyUpvotedOn = pointDto.ReplyUpvotedOn;
            point.ReplyDownvotedBy = pointDto.ReplyDownvotedBy;
            point.ReplyDownvotedOn = pointDto.ReplyDownvotedOn;
            point.RemoveReplyUpvoteBy = pointDto.RemoveReplyUpvoteBy;
            point.RemoveReplyUpvoteOn = pointDto.RemoveReplyUpvoteOn;
            point.RemoveReplyDownvoteBy = pointDto.RemoveReplyDownvoteBy;
            point.RemoveReplyDownvoteOn = pointDto.RemoveReplyDownvoteOn;


            await _context.SaveChangesAsync();

            return pointDto;
        }
        public async Task ThreadCreated(Guid createdBy)
        {
            try
            {
                await Task.FromResult(AddPoints(createdBy, 10));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while assigning points for creating a post.", ex);
            }
        }

        public async Task ThreadUpdated(Guid updatedBy)
        {
            try
            {
                await Task.FromResult(AddPoints(updatedBy, 1));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while assigning points for updating a post.", ex);
            }
        }

        public async Task ThreadDeleted(Guid deletedBy)
        {
            try
            {
                await Task.FromResult(RemovePoints(deletedBy, 1));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while assigning points for deleting a post.", ex);
            }
        }

        public async Task ThreadUpvoted(Guid upVotedBy, long threadId)
        {
            try
            {
                Threads _thread = await Task.FromResult(_context.Threads.Find(threadId));
                if (_thread == null)
                {
                    throw new Exception("Post/Thread not found while assigning points");
                }
                Guid upVotedOn = _thread.CreatedBy;

                await Task.FromResult(AddPoints(upVotedBy, 1));
                await Task.FromResult(AddPoints(upVotedOn, 5));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while assigning points for upvoting a post/thread.", ex);
            }
        }

        public async Task ThreadDownvoted(Guid downVotedBy, long threadId)
        {
            try
            {
                Threads _thread = await Task.FromResult(_context.Threads.Find(threadId));
                if (_thread == null)
                {
                    throw new Exception("Post/Thread not found while assigning points");
                }
                Guid downVotedOn = _thread.CreatedBy;

                await Task.FromResult(RemovePoints(downVotedBy, 1));
                await Task.FromResult(RemovePoints(downVotedOn, 2));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while assigning points for downvoting a post/thread.", ex);
            }
        }

        public async Task RemoveThreadUpvote(Guid upVotedBy, long threadId)
        {
            try
            {
                Threads _thread = await Task.FromResult(_context.Threads.Find(threadId));
                if (_thread == null)
                {
                    throw new Exception("Post/Thread not found while assigning points");
                }
                Guid upVotedOn = _thread.CreatedBy;

                await Task.FromResult(RemovePoints(upVotedBy, 1));
                await Task.FromResult(RemovePoints(upVotedOn, 5));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while assigning points for removing upvote for a post/thread.", ex);
            }
        }

        public async Task RemoveThreadDownvote(Guid downVotedBy, long threadId)
        {
            try
            {
                Threads _thread = await Task.FromResult(_context.Threads.Find(threadId));
                if (_thread == null)
                {
                    throw new Exception("Post/Thread not found while assigning points");
                }
                Guid downVotedOn = _thread.CreatedBy;

                await Task.FromResult(AddPoints(downVotedBy, 1));
                await Task.FromResult(AddPoints(downVotedOn, 2));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while assigning points for removing downvote for a post/thread.", ex);
            }
        }

        public async Task ReplyCreated(Guid createdBy)
        {
            try
            {
                await Task.FromResult(AddPoints(createdBy, 10));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while assigning points for posting a reply.", ex);
            }
        }

        public async Task ReplyUpdated(Guid updatedBy)
        {
            try
            {
                await Task.FromResult(AddPoints(updatedBy, 1));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while assigning points for updating a reply.", ex);
            }
        }

        public async Task ReplyDeleted(Guid deletedBy)
        {
            try
            {
                await Task.FromResult(RemovePoints(deletedBy, 1));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while assigning points for deleting a reply.", ex);
            }
        }

        public async Task ReplyUpvoted(Guid upVotedBy, long replyId)
        {
            try
            {
                Reply _reply = await Task.FromResult(_context.Replies.Find(replyId));
                if(_reply == null)
                {
                    throw new Exception("Reply not found while assigning points");
                }
                Guid upVotedOn = _reply.CreatedBy;

                await Task.FromResult(AddPoints(upVotedBy, 1));
                await Task.FromResult(AddPoints(upVotedOn, 5));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while assigning points for upvoting a reply.", ex);
            }
        }

        public async Task ReplyDownvoted(Guid downVotedBy, long replyId)
        {
            try
            {
                Reply _reply = await Task.FromResult(_context.Replies.Find(replyId));
                if (_reply == null)
                {
                    throw new Exception("Reply not found while assigning points");
                }
                Guid downVotedOn = _reply.CreatedBy;

                await Task.FromResult(RemovePoints(downVotedBy, 1));
                await Task.FromResult(RemovePoints(downVotedOn, 2));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while assigning points for downvoting a reply.", ex);
            }
        }

        public async Task RemoveReplyUpvote(Guid upVotedBy, long replyId)
        {
            try
            {
                Reply _reply = await Task.FromResult(_context.Replies.Find(replyId));
                if (_reply == null)
                {
                    throw new Exception("Reply not found while assigning points");
                }
                Guid upVotedOn = _reply.CreatedBy;

                await Task.FromResult(RemovePoints(upVotedBy, 1));
                await Task.FromResult(RemovePoints(upVotedOn, 5));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while assigning points for removing upvote for a reply.", ex);
            }
        }

        public async Task RemoveReplyDownvote(Guid downVotedBy, long replyId)
        {
            try
            {
                Reply _reply = await Task.FromResult(_context.Replies.Find(replyId));
                if (_reply == null)
                {
                    throw new Exception("Reply not found while assigning points");
                }
                Guid downVotedOn = _reply.CreatedBy;

                await Task.FromResult(AddPoints(downVotedBy, 1));
                await Task.FromResult(AddPoints(downVotedOn, 2));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while assigning points for removing downvote for a post/thread.", ex);
            }
        }

        private async Task AddPoints(Guid userId, int points)
        {
            User _user = await Task.FromResult(_context.Users.Find(userId));
            //Checks if user is valid
            if (_user == null)
            {
                throw new Exception("User not found while assigning points");
            }
            else
            {
                _user.Score += points;
                _context.SaveChanges();
            }
        }

        private async Task RemovePoints(Guid userId, int points)
        {
            User _user = await Task.FromResult(_context.Users.Find(userId));
            //Checks if user is valid
            if (_user == null)
            {
                throw new Exception("User not found while assigning points");
            }
            else
            {
                _user.Score -= points;
                _context.SaveChanges();
            }
        }
    }
}
