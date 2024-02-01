using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.UnitOfWork;

namespace DiscussionForum.Services
{    
    public class CommunityStatusService : ICommunityStatusService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommunityStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<CommunityStatus>> GetCommunityStatusAsync()
        {
            return await Task.FromResult(_unitOfWork.CommunityStatus.GetAll());
        }

        public async Task<CommunityStatus> GetCommunityStatusByIdAsync(int id)
        {
            return await Task.FromResult(_unitOfWork.CommunityStatus.GetById(id));
        }
    }

}
