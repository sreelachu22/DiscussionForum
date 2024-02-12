using DiscussionForum.Models.APIModels;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

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
    }
}
