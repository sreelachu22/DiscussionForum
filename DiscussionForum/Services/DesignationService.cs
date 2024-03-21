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
                throw new ApplicationException("Error occurred while retrieving all designations.", ex);
            }
        }

        public async Task<Designation> GetDesignationByIdAsync(long designationId)
        {
            try
            {
                return await Task.FromResult(_context.Designations.Find(designationId));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while retrieving designation with ID {designationId}.", ex);
            }
        }

        public async Task<Designation> CreateDesignationAsync(string designationName)
        {
            //Creates a new designation and saves it to the database
            try
            {
                Designation _designation = new Designation { DesignationName = designationName, IsDeleted = false };
                _unitOfWork.Designations.Add(_designation);
                _unitOfWork.Complete();
                return _designation;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while creating a designation with name {designationName}.", ex);
            }
        }

        public async Task<Designation> DeleteDesignationAsync(long designationId)
        {
            try
            {
                Designation _designation = await Task.FromResult(_context.Designations.Find(designationId));

                //Checks if the designation is valid and not deleted
                if (_designation != null && !_designation.IsDeleted)
                {
                    _designation.IsDeleted = true;
                    _context.SaveChanges();
                    return _designation;
                }
                //Checks if the designation is valid but deleted
                else if (_designation != null && _designation.IsDeleted)
                {
                    throw new Exception("Designation already deleted.");
                }
                else
                {
                    throw new Exception("Designation not found.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while deleting designation with ID {designationId}.", ex);
            }
        }
    }
}
