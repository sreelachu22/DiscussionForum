using DiscussionForum.Authorization;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
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

        //Get method to get all the users with the options to paginate.
        //Can also sort the users according to its member variables.
        //sort variable takes the member variable according to which sort is done
        //term variable is used as a search term to filter users in a case-insensitive manner.
        //page is for defining the page to return after pagination
        //limit is to specify the number of users needed to show in a page

        [CustomAuth("Head")]
        [HttpGet("GetAllUsersWithPagination")]
        public async Task<IActionResult> GetAllUsers(string term = "", string sort = "name", int page = 1, int limit = 10)
        {
            try
            {
                var userResult = await _userService.GetUsers(term, sort, page, limit);

                // Add pagination headers to the response
                Response.Headers.Add("X-Total-Count", userResult.TotalCount.ToString());
                Response.Headers.Add("X-Total-Pages", userResult.TotalPages.ToString());

                return Ok(userResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        /* get single user*/
        [CustomAuth("User")]
        [HttpGet("{UserId}")]
        public async Task<IActionResult> GetUserById(Guid UserId)
        {
            try
            {
                var userExists = await _userService.GetUserByIDAsync(UserId);

                if (userExists == null)
                    return NotFound();

                return Ok(userExists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }



        /* edit single user role*/
        [CustomAuth("Head")]
        [HttpPut("{UserId}")]
        public async Task<IActionResult> PutUserByIDAsync(Guid UserId, int RoleID, Guid AdminID)
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

        [CustomAuth("User")]
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