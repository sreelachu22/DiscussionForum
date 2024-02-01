using DiscussionForum.Models.EntityModels;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAngularDev")]
    public class DesignationController : ControllerBase
    {
        private readonly IDesignationService _designationService;

        public DesignationController(IDesignationService designationService)
        {
            _designationService = designationService ?? throw new ArgumentNullException(nameof(designationService));
        }

        /// <summary>
        /// Retrieves all designations.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllDesignations()
        {
            try
            {
                var _designations = await _designationService.GetAllDesignationsAsync();
                return Ok(_designations);
            }
            catch (Exception ex)
            {
                //Checks for an inner exception and returns corresponding error message
                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"Error while retrieving all designations \nError: {ex.InnerException.Message}");
                }
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Error while retrieving all designations \nError: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a designation based on the given designation ID.
        /// </summary>
        /// <param name="designationId">The ID of the designation to search for in designations.</param>
        [HttpGet("{designationId}")]
        public async Task<IActionResult> GetDesignationById(long designationId)
        {
            try
            {
                //Validates the request data
                if (designationId <= 0)
                {
                    throw new Exception("Invalid designationId. It should be greater than zero.");
                }

                var _designation = await _designationService.GetDesignationByIdAsync(designationId);

                //Checks if the retrieved designation is null
                if (_designation == null)
                {
                    throw new Exception($"Designation with ID {designationId} not found.");
                }

                return Ok(_designation);
            }
            catch (Exception ex)
            {
                //Checks for an inner exception and returns corresponding error message
                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"Error while retrieving designation with ID = {designationId} \nError: {ex.InnerException.Message}");
                }
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Error while retrieving designation with ID = {designationId} \nError: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new designation.
        /// </summary>
        /// <param name="designationName">The name of the new designation.</param>
        [HttpPost]
        public async Task<IActionResult> CreateDesignation([FromBody] string designationName)
        {
            try
            {
                //Validates the request data
                if (string.IsNullOrWhiteSpace(designationName))
                {
                    throw new Exception("Invalid designationName. It cannot be null or empty.");
                }

                var _designation = await _designationService.CreateDesignationAsync(designationName);
                return Ok(_designation);
            }
            catch (Exception ex)
            {
                //Checks for an inner exception and returns corresponding error message
                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"Error while creating designation \nError: {ex.InnerException.Message}");
                }
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Error while creating designation \nError: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a designation based on the given designation ID.
        /// </summary>
        /// <param name="designationId">The ID of the designation to delete.</param>
        [HttpDelete("{designationId}")]
        public async Task<IActionResult> DeleteDesignation(long designationId)
        {
            try
            {
                //Validates the request data
                if (designationId <= 0)
                {
                    throw new Exception("Invalid designationId. It should be greater than zero.");
                }

                var _designation = await _designationService.DeleteDesignationAsync(designationId);
                return Ok(_designation);
            }
            catch (Exception ex)
            {
                //Checks for an inner exception and returns corresponding error message
                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"Error while deleting designation with ID = {designationId} \nError: {ex.InnerException.Message}");
                }
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Error while deleting designation with ID = {designationId} \nError: {ex.Message}");
            }
        }
    }
}
