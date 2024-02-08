using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.UnitOfWork;

namespace DiscussionForum.Services
{
    public class PointService : IPointService
    {
        private readonly AppDbContext _context;

        public PointService(AppDbContext context)
        {
            _context = context;
        }
        public async Task PostCreated(Guid guid)
        {
            try
            {
                await Task.FromResult(AddPoints(guid, 10));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while assigning points for creating a post.", ex);
            }
        }

        public async Task PostUpdated(Guid guid)
        {
            try
            {
                await Task.FromResult(AddPoints(guid, 1));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while assigning points for updating a post.", ex);
            }
        }

        public async Task PostDeleted(Guid guid)
        {
            try
            {
                await Task.FromResult(RemovePoints(guid, 1));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while assigning points for deleting a post.", ex);
            }
        }

        public async Task ReplyCreated(Guid guid)
        {
            try
            {
                await Task.FromResult(AddPoints(guid, 10));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while assigning points for posting a reply.", ex);
            }
        }

        public async Task ReplyUpdated(Guid guid)
        {
            try
            {
                await Task.FromResult(AddPoints(guid, 1));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while assigning points for updating a reply.", ex);
            }
        }

        public async Task ReplyDeleted(Guid guid)
        {
            try
            {
                await Task.FromResult(RemovePoints(guid, 1));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while assigning points for deleting a reply.", ex);
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
