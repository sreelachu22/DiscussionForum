using DiscussionForum.Data;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using DiscussionForum.Type;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DiscussionForum.Services
{
    public class LoginService : ILoginService
    {
        private readonly AppDbContext _db;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public LoginService(AppDbContext db,
                            SignInManager<User> signInManager,
                            IConfiguration configuration,
                            UserManager<User> userManager)
        {
            _db = db;
            _signInManager = signInManager;
            _configuration = configuration;
            _userManager = userManager;
        }

        /// <summary>
        /// Attempts to log in an admin user asynchronously.
        /// </summary>
        /// <param name="dto">The login information provided by the user.</param>
        /// <returns>A service response containing a token if the login is successful, or an error message otherwise.</returns>
        public async Task<ServiceResponse<string>> AdminLoginAsync(LoginDto dto)
        {
            var response = new ServiceResponse<string>();
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                response.AddError("", "Email entered not found.");
                return response;
            }
            var signin = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, true);
            if (signin.Succeeded)
            {
                response.Result = GenerateToken(user);
                return response;
            }
            response.AddError("", "Unable to login, recheck your email and password.");
            return response;
        }

        /// <summary>
        /// Generates a JSON Web Token (JWT) for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        /// <param name="expiry">Optional. The expiry time for the token from the external provider. If not provided, a default expiry time of 1 day from the current UTC time will be used.</param>
        /// <returns>A string representing the generated JWT.</returns>
        public string GenerateToken(User user)
        {
            string key = _configuration["Jwt:Key"];
            string issuer = _configuration["Jwt:Issuer"];
            string audience = _configuration["Jwt:Audience"];
            var role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().First();
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(signingKey, "HS256");
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, role),
                new Claim("Role", role),
                new Claim("UserId", user.Id.ToString()),
            };
            var token = new JwtSecurityToken(
                issuer: issuer,
                claims: claims,
                audience: audience,
                expires: DateTime.UtcNow + TimeSpan.FromDays(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /*public async Task<ServiceResponse<string>> ExternalAuthenticationAsync(string token, string provider)
        {
            var response = new ServiceResponse<string>();
            var newToken = new JwtSecurityToken(token);
            var claims = newToken.Claims;
            string email = claims.First(c => c.Type == "preferred_username").Value;
            string oid = claims.First(c => c.Type == "oid").Value;
            string fullName = claims.First(c => c.Type == "name").Value;
            string[] nameParts = fullName.Split(' ');
            string firstName = nameParts[0];
            string[] lastNameParts = nameParts.Skip(1).ToArray();
            string lastName = string.Join(" ", lastNameParts);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                response.AddError("", "Your authorization failed.Please try refreshing the page and fill in the correct credentials.");
                return response;
            }
            var demo = await _db.UserLog.FirstOrDefaultAsync(c => c.UserID == user.Id);
            if (demo == null)
            {
                user.Name = firstName;
                await _db.SaveChangesAsync();
                var loginInfo = new UserLoginInfo(provider, oid, provider);
                var signin = await _userManager.AddLoginAsync(user, loginInfo);
                if (signin.Succeeded)
                {
                    response.Result = GenerateToken(user);
                    return response;
                }
                response.AddError("", "Your authorization failed. Please try refreshing the page and fill in the correct credentials.");
                return response;
            }
            else
            {
                response.Result = GenerateToken(user);
                return response;
            }
        }*/
    }
}
