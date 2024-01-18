using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.UnitOfWork;

namespace DiscussionForum.Services
{
    public class DesignationService : IDesignationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public DesignationService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IEnumerable<Designation>> GetAllDesignationsAsync()
        {
            try
            {
                return await Task.FromResult(_unitOfWork.Designations.GetAll());
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                throw new ApplicationException("Error occurred while retrieving all designations.", ex);
            }
        }

        public async Task<Designation> GetDesignationByIdAsync(long designationID)
        {
            try
            {
                return await Task.FromResult(_context.Designations.Find(designationID));
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                throw new ApplicationException($"Error occurred while retrieving designation with ID {designationID}.", ex);
            }
        }

        public async Task<Designation> CreateDesignationAsync(string designationName)
        {
            try
            {
                return await Task.FromResult(CreateDesignation(designationName));
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                throw new ApplicationException($"Error occurred while creating a designation with name {designationName}.", ex);
            }
        }

        private Designation CreateDesignation(string designationName)
        {
            Designation designation = new Designation { DesignationName = designationName, IsDeleted = false };
            _unitOfWork.Designations.Add(designation);
            _unitOfWork.Complete();
            return designation;
        }

        public async Task DeleteDesignationAsync(long designationID)
        {
            try
            {
                await Task.Run(() => DeleteDesignation(designationID));
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                throw new ApplicationException($"Error occurred while deleting designation with ID {designationID}.", ex);
            }
        }

        private void DeleteDesignation(long designationID)
        {
            var designation = _context.Designations.Find(designationID);

            if (designation != null)
            {
                designation.IsDeleted = true;
                _context.SaveChanges();
            }
        }
    }
}
