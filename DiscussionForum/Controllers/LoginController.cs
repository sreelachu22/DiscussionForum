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
        /// POST - /api/Accounts/Login
        [HttpPost("Login")]
        public async Task<IActionResult> AdminLogin(LoginDto dto)
        {
            var res = await _loginService.AdminLoginAsync(dto);
            return Ok(res);
        }
    }
}
