using System.Net;
using System.Text.Json;
using DotnetRpg.Models.Exceptions;

namespace DotnetRpg;

public class GlobalExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILogger _logger;

    public GlobalExceptionHandlerMiddleware(ILogger logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request.");
            
            HttpStatusCode statusCode;
            string errorTitle;

            switch (ex)
            {
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    errorTitle = "Resource not found";
                    break;
                case BadRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorTitle = "Invalid request";
                    break;
                case ConflictException:
                    statusCode = HttpStatusCode.Conflict;
                    errorTitle = "Resource already exists";
                    break;
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    errorTitle = "Unauthorized";
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    errorTitle = "Server error";
                    break;
            }

            var errorDetails = new ErrorDetails((int)statusCode, errorTitle, ex.Message);
            var responseContent = JsonSerializer.Serialize(errorDetails);

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(responseContent);
        }
    }

    private record ErrorDetails(int StatusCode, string Title, string Message);
}
