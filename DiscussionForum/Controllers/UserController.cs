using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    [ApiController]
    [EnableCors("AllowAngularDev")]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) { 
            _userService = userService;
        }

        [HttpGet("{UserId}")]
        public async Task<IActionResult> GetCommunityCategoryById(Guid UserId)
        {
            var userExists = await _userService.GetUserByIDAsync(UserId);

            if (userExists == null)
                return NotFound();

            return Ok(userExists);
        }
    }
}
