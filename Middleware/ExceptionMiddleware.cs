
using System.Net;
using API.Models;

namespace API.Middleware;

public class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
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
            var error = HandleException(ex);

            ApiResponse apiResponse = new ApiResponse
            {
                StatusCode = (int)error.StatusCode,
                IsSuccess = false,
                Error = error,
                Result = null
            };

            context.Response.StatusCode = (int)error.StatusCode;
            await context.Response.WriteAsJsonAsync(apiResponse);
        }
    }

    private ErrorResponse HandleException(Exception exception)
    {
        int statusCode = StatusCodes.Status500InternalServerError;
        string errorMessage = "Internal server error";
        string exceptionMessage = exception.Message;

        switch (exception)
        {
            case BadHttpRequestException:
                statusCode = StatusCodes.Status400BadRequest;
                errorMessage = $"Bad request: {exceptionMessage}";
                break;
            case UnauthorizedAccessException:
                statusCode = StatusCodes.Status401Unauthorized;
                errorMessage = "Unauthorized access";
                break;
            case KeyNotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                errorMessage = $"{exceptionMessage} not found";
                break;
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                errorMessage = "Internal server error";
                break;
        }

        var error = new ErrorResponse
        {
            Message = errorMessage,
            StatusCode = (HttpStatusCode)statusCode,
            StackTrace = exception.StackTrace
        };

        return error;
    }
}