using System.Net;
using System.Text.Json;
using FluentValidation;

namespace ProjectFlow.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        var response = context.Response;
        response.ContentType = "application/json";

        var statusCode = HttpStatusCode.InternalServerError;
        object errorResponse;

        if (exception is ValidationException validationException)
        {
            statusCode = HttpStatusCode.BadRequest; 
            errorResponse = new
            {
                title = "Validation Error",
                status = (int)statusCode,
                errors = validationException.Errors
                    .GroupBy(e => e.PropertyName, StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
            };
        }
        else
        {
            statusCode = HttpStatusCode.InternalServerError; 
            errorResponse = new
            {
                title = "An Internal Server Error Occurred",
                status = (int)statusCode,
                detail = "An unexpected error occurred. Please try again." 
            };
        }

        response.StatusCode = (int)statusCode;
        await response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}