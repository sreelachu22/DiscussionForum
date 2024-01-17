using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.UnitOfWork;

namespace DiscussionForum.Services
{
    public class ForumStatusService : IForumStatusService
    {
        private readonly IUnitOfWork _unitOfWork;


        public ForumStatusService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ForumStatus>> GetForumStatusAsync()
        {
            return await Task.FromResult(_unitOfWork.ForumStatus.GetAll());
        }

        public async Task<ForumStatus> GetForumStatusByIdAsync(int id)
        {
            return await Task.FromResult(_unitOfWork.ForumStatus.GetById(id));
        }


    }
}
