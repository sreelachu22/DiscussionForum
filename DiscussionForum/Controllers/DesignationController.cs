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
            _designationService = designationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDesignations()
        {
            var designations = await _designationService.GetAllDesignationsAsync();
            return Ok(designations);
        }

        [HttpGet("{designationId}")]
        public async Task<IActionResult> GetDesignationById(long designationId)
        {
            var designation = await _designationService.GetDesignationByIdAsync(designationId);

            if (designation == null)
                return NotFound();

            return Ok(designation);
        }

        [HttpPost("{designationName}")]
        public async Task<IActionResult> CreateDesignation(string designationName)
        {
            var designation = await _designationService.CreateDesignationAsync(designationName);
            return Ok(designation);
        }

        [HttpDelete("{designationId}")]
        public async Task<IActionResult> DeleteDesignation(long designationId)
        {
            await _designationService.DeleteDesignationAsync(designationId);
            return NoContent();
        }
    }
}
