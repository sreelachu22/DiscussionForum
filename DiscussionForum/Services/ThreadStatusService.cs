using DiscussionForum.Models.EntityModels;
using DiscussionForum.Services;
using DiscussionForum.UnitOfWork;

namespace DiscussionForum.Services
{
    public class ThreadStatusService : IThreadStatusService
    {
        private readonly IUnitOfWork _unitOfWork;

        // ThreadStatusService: A service class for handling operations related to thread statuses.

        // Constructor: Initializes the ThreadStatusService with the required dependencies.
        // - _unitOfWork: Unit of work for managing database transactions.

        public ThreadStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GetThreadStatusAsync: Retrieves all thread statuses asynchronously.
        // - Returns: A collection of thread statuses.

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

        // GetThreadStatusByIdAsync: Retrieves a thread status by its ID asynchronously.
        // - Parameters: Thread status ID.
        // - Returns: The thread status with the specified ID.

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
