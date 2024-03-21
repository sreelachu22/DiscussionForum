using DiscussionForum.Models.EntityModels;

namespace DiscussionForum.Services
{
    public interface IDesignationService
    {
        /// <summary>
        /// Retrieves all designations.
        /// </summary>
        Task<IEnumerable<Designation>> GetAllDesignationsAsync();

        /// <summary>
        /// Retrieves a designation based on the given designation ID.
        /// </summary>
        /// <param name="designationID">The ID to search for in designations.</param>
        Task<Designation> GetDesignationByIdAsync(long designationID);

        /// <summary>
        /// Creates a new designation with the given designation name.
        /// </summary>
        /// <param name="designationName">The name of the new designation.</param>
        Task<Designation> CreateDesignationAsync(string designationName);

        /// <summary>
        /// Deletes a designation based on the given designationId.
        /// </summary>
        /// <param name="designationID">The ID of the designation to delete.</param>
        Task<Designation> DeleteDesignationAsync(long designationID);

    }
}
