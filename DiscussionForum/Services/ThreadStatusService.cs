//using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Services;
//using DiscussionForum.UnitOfWork.ThreadStatusService.Infrastructure.UnitOfWork;
using DiscussionForum.UnitOfWork;

namespace DiscussionForum.Services
{
    public class ThreadStatusService : IThreadStatusService
    {
        private readonly IUnitOfWork _unitOfWork;
        

        public ThreadStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task <IEnumerable<ThreadStatus>> GetThreadStatusAsync()
        {
            try
            {
                return await Task.FromResult(_unitOfWork.ThreadStatus.GetAll());
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while trying to get thread status", ex);
            }

        }

        public async Task <ThreadStatus> GetThreadStatusByIdAsync(int id)
        {
            try
            {
                return await Task.FromResult(_unitOfWork.ThreadStatus.GetById(id));
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while trying to get thread status using ID {id}.", ex);
            }

        }
    }
}
