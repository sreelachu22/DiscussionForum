using DiscussionForum.Authorization;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    [ApiController]
    [EnableCors("AllowAngularDev")]
    [Route("api/[controller]")]
    public class PointController : ControllerBase
    {
        private readonly IPointService _pointService;

        public PointController(IPointService pointService)
        {
            _pointService = pointService;
        }

        /*[CustomAuth("User")]*/
        [HttpPut("{communityID}")]
        public async Task<IActionResult> UpdatePoint(int communityID, [FromBody] PointDto pointDto)
        {
            var result = await _pointService.UpdatePoint(communityID, pointDto);
            if (result == null)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        /*[CustomAuth("User")]*/
        [HttpGet("{communityId}")]
        public async Task<ActionResult<PointDto>> GetPointsByCommunityId(int communityId)
        {
            try
            {
                var pointDto = await _pointService.GetPointsByCommunityId(communityId);
                return Ok(pointDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
