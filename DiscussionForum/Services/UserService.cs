using DiscussionForum.UnitOfWork;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscussionForum.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        

        public UserService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        

        public async Task<User> GetUserByIDAsync(Guid Userid)
        {
            try
            {
                var user= await Task.FromResult(_context.Users.Find(Userid));
                var a = user;
                return user; 
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching User.", ex);
            }
        }

        

        
        

        
    }
}
