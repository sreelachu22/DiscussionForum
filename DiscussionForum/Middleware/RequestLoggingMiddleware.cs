using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
    /// <param name="context">The HttpContext representing the incoming HTTP request.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        // Log the request details
        string logMessage = $"{DateTime.Now} - {context.Request.Method} {context.Request.Path}";
        Console.WriteLine(logMessage);
        await LogRequestToFile(logMessage);

        // Call the next middleware in the pipeline
        await _next(context);
    }

    /// <summary>
    /// Logs the request details to a file.
    /// </summary>
    /// <param name="logMessage">The log message to be written to the file.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task LogRequestToFile(string logMessage)
    {
        // Specify the file path where you want to log the requests
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Middleware", "requestlog.txt");

        // Ensure the directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        // Append the log message to the file
        await File.AppendAllTextAsync(filePath, $"{logMessage}{Environment.NewLine}", Encoding.UTF8);
    }
}
