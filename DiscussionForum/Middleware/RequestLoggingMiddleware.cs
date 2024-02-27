using DiscussionForum.Data;
using DiscussionForum.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Text;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invokes the middleware to log incoming HTTP requests.
    /// </summary>
    /// <param name="httpContext">The HttpContext representing the incoming HTTP request.</param>
    /// <param name="dbContext">The AppDbContext representing the database connection.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext httpContext, AppDbContext dbContext)
    {
        // Log the request details
        string _logMessage = $"{DateTime.Now} - {httpContext.Request.Method} {httpContext.Request.Path}";
        Console.WriteLine(_logMessage);
        await LogRequestToFile(_logMessage);

        await LogRequestToDb(httpContext, dbContext);

        // Call the next middleware in the pipeline
        await _next(httpContext);
    }

    /// <summary>
    /// Logs the request details to a file.
    /// </summary>
    /// <param name="logMessage">The log message to be written to the file.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task LogRequestToFile(string logMessage)
    {
        // Specify the file path where you want to log the requests
        string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Middleware", "requestlog.txt");

        // Ensure the directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath));

        // Append the log message to the file
        await File.AppendAllTextAsync(_filePath, $"{logMessage}{Environment.NewLine}", Encoding.UTF8);
    }

    /// <summary>
    /// Logs the request details according to the UserLog to the database
    /// </summary>
    /// <param name="httpContext">The HttpContext representing the incoming HTTP request.</param>
    /// <param name="dbContext">The AppDbContext representing the database connection.</param>
    private async Task LogRequestToDb(HttpContext httpContext, AppDbContext dbContext)
    {
        // Retrieve the authorization header from the request
        string _authHeader = httpContext.Request.Headers["authorization"];

        // Check if the authorization header exists and starts with "Bearer"
        if (!string.IsNullOrEmpty(_authHeader) && _authHeader.StartsWith("Bearer"))
        {
            // Extract the token from the authorization header
            string _token = _authHeader.Substring("Bearer ".Length).Trim();

            // Initialize variables
            Guid _userId;

            try
            {
                // Decode the token to retrieve claims
                var _tokenHandler = new JwtSecurityTokenHandler();
                var _decodedToken = _tokenHandler.ReadJwtToken(_token);

                // Retrieve the user ID claim
                var _nameIdentifierClaim = _decodedToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (_nameIdentifierClaim != null)
                {
                    // Extract the value of the claim
                    _userId = Guid.Parse(_nameIdentifierClaim.Value);
                }
                else
                {
                    throw new Exception("User ID claim not found in token");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error decoding token or retrieving user ID from claims", ex);
            }

            try
            {
                // Perform logging with the retrieved user ID
                long _lastUserLogId = dbContext.UserLog
                    .OrderByDescending(ul => ul.LoginTime)
                    .FirstOrDefault(ul => ul.UserID == _userId)
                    .UserLogID;

                UserRequestLog _userRequestLog = new UserRequestLog
                {
                    UserLogID = _lastUserLogId,
                    RequestTime = DateTime.Now,
                    RequestMethod = httpContext.Request.Method,
                    RequestPath = httpContext.Request.Path,
                    ResponseStatusCode = httpContext.Response.StatusCode,
                    IsDeleted = false,
                    CreatedBy = _userId,
                    CreatedAt = DateTime.Now,
                };

                dbContext.UserRequestLog.Add(_userRequestLog);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error logging request to the database: {ex.Message}");
                // Optionally rethrow the exception or handle it as needed
                throw new Exception($"Error logging request to the database{ex.Message}");
            }
        }
    }

}
