using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{

    [Route("api/users")]
    [ApiController]
    [EnableCors("AllowAngularDev")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(string term, string sort, int page = 1, int limit = 10)
        {
            var userResult = await _userService.GetUsers(term, sort, page, limit);

            // Add pagination headers to the response
            Response.Headers.Add("X-Total-Count", userResult.TotalCount.ToString());
            Response.Headers.Add("X-Total-Pages", userResult.TotalPages.ToString());
            return Ok(userResult.Users);
         }
        [HttpGet("{UserId}")]
        public async Task<IActionResult> GetUserById(Guid UserId)
        {
            var userExists = await _userService.GetUserByIDAsync(UserId);

            if (userExists == null)
                return NotFound();

            return Ok(userExists);
        }
    }
}
