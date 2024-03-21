using DiscussionForum.Models.APIModels;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    [ApiController]
    [EnableCors("AllowAngularDev")]
    [Route("api/[controller]")]
    public class BadgeController : ControllerBase
    {
        private readonly IBadgeService _badgeService;

        public BadgeController(IBadgeService badgeService)
        {
            _badgeService = badgeService;
        }

        /*[CustomAuth("User")]*/
        [HttpPut("{communityID}")]
        public async Task<IActionResult> UpdateBadge(int communityID, [FromBody] BadgeDto badgeDto)
        {
            var result = await _badgeService.UpdateBadges(communityID, badgeDto);

            if (result == null)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        /*[CustomAuth("User")]*/
        [HttpGet("{communityId}")]
        public async Task<ActionResult<BadgeDto>> GetBadges(int communityId)
        {
            try
            {
                var badgeDto = await _badgeService.GetBadgesByCommunityId(communityId);

                return Ok(badgeDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
