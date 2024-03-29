﻿using DiscussionForum.Data;
using DiscussionForum.Models.APIModels;
using DiscussionForum.Models.EntityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DiscussionForum.Services
{
    public class LoginService : ILoginService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public LoginService(AppDbContext db,
                            IConfiguration configuration)
        {
            _context = db;
            _config = configuration;
        }



        /// <summary>
        /// Attempts to log in an admin user asynchronously.
        /// </summary>
        /// <param name="userLogin">The login information provided by the user.</param>
        /// <returns>A service response containing a token if the login is successful, or null otherwise.</returns>
        public async Task<TokenDto> AdminLoginAsync(AdminLoginDto userLogin)
        {
            string Password = DotNetEnv.Env.GetString("AdminPassword");
                string adminPassword = _config.GetValue<string>("SuperAdmin:Password");
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userLogin.Email);

            if (user != null)
            {
                if (string.Equals(HashPassword(userLogin.Password), HashPassword(adminPassword), StringComparison.Ordinal))
                {
                    var userName = userLogin.Email;
                    string tokengenerated = await TokenGenerater(user);

                    return new TokenDto { Token = tokengenerated };
                }

                return null;
            }

            return null;
        }

        /// <summary>
        /// Generates a hash for the provided password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>The hashed password.</returns>
        private string HashPassword(string password)
        {

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        /// <summary>
        /// Generates a JWT token for the given user.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        /// <returns>The generated JWT token.</returns>
        public async Task<string> TokenGenerater(User user)
        {
            string key = _config["Jwt:JWT_Key"];
            string issuer = _config["Jwt:JWT_Issuer"];
            string audience = _config["Jwt:JWT_Audience"];
            var role = await GetRole(user);
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, role.ToString()),
                new Claim("Role", role.ToString()),
                new Claim("UserId", user.UserID.ToString()),
            };
            var token = new JwtSecurityToken(
                issuer: issuer,
                claims: claims,
                audience: audience,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Retrieves the role of the specified user.
        /// </summary>
        /// <param name="user">The user whose role is to be retrieved.</param>
        /// <returns>The role of the user.</returns>
        private async Task<string> GetRole(User user)
        {
            var roleName = await _context.Users
                .Where(u => u.UserID == user.UserID)
                .Join(_context.UserRoleMapping,
                      u => u.UserID,
                      ur => ur.UserID,
                      (u, ur) => ur)
                .Join(_context.Roles,
                      ur => ur.RoleID,
                      r => r.RoleID,
                      (ur, r) => r.RoleName)
                .FirstOrDefaultAsync();

            return roleName;
        }

        /// <summary>
        /// Performs external authentication using the provided token and provider.
        /// </summary>
        /// <param name="token">The token provided by the external authentication provider.</param>
        /// <param name="provider">The external authentication provider.</param>
        /// <returns>A service response containing a token if the authentication is successful, or null otherwise.</returns>
        public async Task<TokenDto> ExternalAuthenticationAsync(string token, string provider)
        {

            try
            {
                var newToken = new JwtSecurityToken(token);
                var claims = newToken.Claims;
                // retrieve name, role, email from azure token claims
                string name = claims.FirstOrDefault(c => c.Type == "name")?.Value;
                string[] roles = claims.Where(c => c.Type == "roles").Select(c => c.Value).ToArray();
                string roleName = roles[0];
                string email = claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;
                

                if (string.IsNullOrEmpty(email))
                {
                    return null;
                }

                else
                {
                    var user = await _context.Users.Where(us => us.Email == email).FirstOrDefaultAsync();
                    var systemUserId = await _context.Users
                        .Where(us => us.Email == "system@example.com")
                        .Select(us => us.UserID)
                        .FirstOrDefaultAsync();

                    if (user == null)
                    {
                        // User does not exist, create a new user
                        user = new User
                        {
                            Email = email,
                            Name = name,
                            Score = 0,
                            DepartmentID = 1,
                            DesignationID = 1,
                            IsDeleted = false,
                            CreatedBy = systemUserId,
                            CreatedAt = DateTime.Now,
                            ModifiedBy = null,
                            ModifiedAt = null,
                        };

                        _context.Users.Add(user);
                        await _context.SaveChangesAsync();

                        var newUserId = (from u in _context.Users
                                         where u.Email == email
                                         select u.UserID).FirstOrDefault();
                        int roleId;

                        if (roleName == "SuperAdmin")
                        {
                            roleId = 1;
                        }
                        else if (roleName == "CommunityHead")
                        {
                            roleId = 2;
                        }
                        else if (roleName == "User")
                        {
                            roleId = 3;
                        }
                        else
                        {
                            roleId = 3; // Default value is RoleID of User (3)
                        }

                        var userRoleMapping = new UserRoleMapping
                        {
                            UserID = newUserId,
                            RoleID = roleId,
                            IsDeleted = false,
                            CreatedBy = systemUserId,
                            CreatedAt = DateTime.Now,
                            ModifiedBy = null,
                            ModifiedAt = null,
                        };



                        _context.UserRoleMapping.Add(userRoleMapping);
                        await _context.SaveChangesAsync();
                    }

                    
                    // Generate token for the user
                    var result = new TokenDto { Token = await TokenGenerater(user) };
                    await LogUserLogin(user.UserID);
                    return result;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Logs the login time of the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user to log.</param>
        public async Task LogUserLogin(Guid userId)
        {
            var userLog = new UserLog
            {
                UserID = userId,
                LoginTime = DateTime.Now,
                IsDeleted = false,
                CreatedBy = userId,
                CreatedAt = DateTime.Now
            };

            _context.UserLog.Add(userLog);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Logs the logout time of the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user to log out.</param>
        public async Task LogUserLogout(Guid userId)
        {
            try
            {
                // Find the user log entry for the given user ID
                var userLog = await _context.UserLog.FirstOrDefaultAsync(log => log.UserID == userId && log.LogoutTime == null);

                if (userLog != null)
                {
                    // Update the logout time for the user
                    userLog.LogoutTime = DateTime.Now;

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log exceptions
                Console.WriteLine($"Error occurred during logout: {ex.Message}");
            }
        }


    }
}
