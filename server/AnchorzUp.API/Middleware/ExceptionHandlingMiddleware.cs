using System.Net;
using System.Text.Json;

namespace AnchorzUp.API.Middleware;

public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> _logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            ArgumentNullException => HttpStatusCode.BadRequest, // 400
            ArgumentException => HttpStatusCode.BadRequest, // 400
            InvalidOperationException => HttpStatusCode.BadRequest, // 400
            KeyNotFoundException => HttpStatusCode.NotFound, // 404
            _ => HttpStatusCode.InternalServerError // 500
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            success = false,
            message = exception.Message,
            statusCode = context.Response.StatusCode,
            timestamp = DateTime.UtcNow
        };

        var jsonResponse = JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(jsonResponse);
    }
}
