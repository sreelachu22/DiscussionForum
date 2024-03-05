using DiscussionForum.ExceptionFilter;
using DiscussionForum.Models.APIModels;
using Newtonsoft.Json;
using Serilog;

namespace DiscussionForum.Middleware.ExceptionLogging
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleException(ex, httpContext);
            }
        }

        private async Task HandleException(Exception ex, HttpContext httpContext)
        {
            if(ex.InnerException != null)
            {
                Log.Error(ex.InnerException.Message);
            }
            else
            {
                Log.Error(ex.Message);
            }

            if (ex is InvalidOperationException)
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsJsonAsync(new ErrorDTO
                {
                    Message = "Invalid operation",
                    StatusCode = 400,
                    Success = false
                });
            }
            else if (ex is ArgumentException)
            {
                await httpContext.Response.WriteAsync("Invalid argument");
            }
            else if (ex is CustomException _customException)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = _customException.statusCode;
                await httpContext.Response.WriteAsJsonAsync(new ErrorDTO
                {
                    Message = _customException.message,
                    StatusCode = _customException.statusCode,
                    Success = false
                });
            }
            else
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = 409;
            }
        }
    }
}
