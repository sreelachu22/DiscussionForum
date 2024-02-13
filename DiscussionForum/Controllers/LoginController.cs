using DiscussionForum.Models.APIModels;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DiscussionForum.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAngularDev")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        /// <summary>
        /// Endpoint for administratively logging in a user.
        /// </summary>
        /// <param name="dto">The login information provided by the user.</param>
        /// <returns>An IActionResult representing the result of the login operation.</returns>
        /// POST - /api/Login
        [HttpPost("login")]
        public async Task<IActionResult> AdminLogin([FromBody] AdminLoginDto dto)
        {
            TokenDto ExistingUserToken = await _loginService.AdminLoginAsync(dto);

            if (ExistingUserToken != null)
            {
                return Ok(ExistingUserToken);
            }
            return Conflict("Failed to Log in user.");

        }

        /// <summary>
        /// Endpoint for performing external authentication.
        /// </summary>
        /// <param name="dto">The external authentication information provided by the user.</param>
        /// <returns>An IActionResult representing the result of the external authentication operation.</returns>
        /// POST - /api/Accounts/ExternalLogin
        [HttpPost("ExternalLogin")]
        public async Task<IActionResult> ExternalAuthentication(ExternalAuthDto dto)
        {
            var res = await _loginService.ExternalAuthenticationAsync(dto.Token, dto.Provider);
            if (res == null)
            {
                return new UnauthorizedResult();
            }
            return Ok(res);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(Guid userId)
        {
            try
            {
                await _loginService.LogUserLogout(userId);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "An error occurred while logging out user.");
            }
        }
    }
}
