using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.UnitOfWork;
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
        public async Task PostCreated(Guid createdBy)
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

        public async Task PostUpdated(Guid updatedBy)
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

        public async Task PostDeleted(Guid deletedBy)
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

        private async Task AddPoints(Guid userId, int points)
        {
            User _user = await Task.FromResult(_context.Users.Find(userId));
            //Checks if user is valid
            if (_user == null)
            {
                throw new Exception("User not found");
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
                throw new Exception("User not found");
            }
            else
            {
                _user.Score -= points;
                _context.SaveChanges();
            }
        }
    }
}
