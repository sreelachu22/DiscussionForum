namespace DiscussionForum.Services
{
    using DiscussionForum.Models.EntityModels;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<User> GetUserByIDAsync(Guid UserID);
    }
}

