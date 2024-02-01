using DiscussionForum.Models.APIModels;
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

            return Ok(userResult);
        }


       /* get single user*/
        [HttpGet("{UserId}")]
        public async Task<IActionResult> GetUserById(Guid UserId)
        {
            var userExists = await _userService.GetUserByIDAsync(UserId);

            if (userExists == null)
                return NotFound();

            return Ok(userExists);

        }


        /* edit single user role*/
        [HttpPut("{UserId}")]
        public async Task<IActionResult> PutUserByIDAsync(Guid UserId,int RoleID,Guid AdminID)
        {
            try
            {
                await _userService.PutUserByIDAsync(UserId, RoleID, AdminID);
                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating user role");
            }
        }

        //leaderboard

        [HttpGet("TopUsersByScore/{limit}")]
        public async Task<ActionResult<List<SingleUserDTO>>> GetTop5UsersByScore(int limit)
        {
            try
            {
                var topUsers = await _userService.GetTopUsersByScoreAsync(limit);
                return Ok(topUsers);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
