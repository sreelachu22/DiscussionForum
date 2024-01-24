using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using DiscussionForum.Repositories;

namespace DiscussionForum.Services
{
    public class CommunityService : ICommunityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public CommunityService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
        }


        public Task<IEnumerable<Community>> GetAllCommunities()
        {
            try
            {
                var communities = _unitOfWork.Community.GetAll();
                return Task.FromResult(communities);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new ApplicationException("Error occurred while fetching Roles", ex);
            }

        }

        /*public Task<Role> GetRoleByID(int id)
        {
            try
            {
                return Task.FromResult(_unitOfWork.Role.GetById(id));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occurred while fetching Roles", ex);
            }
        }*/


    }
}
