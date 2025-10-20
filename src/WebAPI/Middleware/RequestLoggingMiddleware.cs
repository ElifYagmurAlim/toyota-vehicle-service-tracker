using System.Diagnostics;

namespace VehicleServiceTracker.WebAPI.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;
    private readonly string _logFilePath;

    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _logFilePath = Path.Combine(env.ContentRootPath, "Logs", "requests.txt");
        
        Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath)!);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestPath = context.Request.Path;
        var requestMethod = context.Request.Method;

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            
            var logMessage = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] " +
                           $"{requestMethod} {requestPath} - " +
                           $"Status: {context.Response.StatusCode} - " +
                           $"Duration: {stopwatch.ElapsedMilliseconds}ms";

            _logger.LogInformation(logMessage);
            
            await File.AppendAllTextAsync(_logFilePath, logMessage + Environment.NewLine);
        }
    }
}