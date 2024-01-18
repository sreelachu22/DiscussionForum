using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Services
{
    public interface IDesignationService
    {
        Task<IEnumerable<Designation>> GetAllDesignationsAsync();
        Task<Designation> GetDesignationByIdAsync(long _designationID);
        Task<Designation> CreateDesignationAsync(string _designationName);
        Task DeleteDesignationAsync(long _designationID);

    }
}
