/*using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    [Route("api/users")]
    [ApiController]
    [EnableCors("AllowAngularDev")]
    public class UserRoleMappingController
    {
        private readonly IUserRoleMappingService _userRoleMappingService;

        public UsersController(IUserRoleMappingService userRoleMappingService)
        {
            _userRoleMappingService = userRoleMappingService;
        }
        [HttpPut]
        public async Task<IActionResult> SetUserRole(Guid userId, long newRoleId)
        {
            try
            {
                await_userRoleMappingService.SetUserRoleAsync(userId, newRoleId);
                return Ok("User role updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
*/