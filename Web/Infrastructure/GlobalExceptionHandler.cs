using System.ComponentModel.DataAnnotations;
using Application.Common.Model;
using Microsoft.AspNetCore.Diagnostics;
using Npgsql;
using ValidationException = FluentValidation.ValidationException;

namespace Web.Infrastructure;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers = new()
    {
        { typeof(KeyNotFoundException), HandleKeyNotFoundExceptionAsync },
        { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessExceptionAsync },
        {typeof(PostgresException), HandlePostgresExceptionAsync},
        { typeof(ValidationException), HandleValidationExceptionAsync }
    };
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionType = exception.GetType();
        if (!_exceptionHandlers.TryGetValue(exceptionType, out var handler))
        { 
            return false;
        }
        logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);
        await _exceptionHandlers[exceptionType].Invoke(httpContext, exception);
        return true;
    }
    
    private static async Task HandleKeyNotFoundExceptionAsync(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        await httpContext.Response.WriteAsJsonAsync(Result.FailureResponse(StatusCodes.Status404NotFound,"The specified resource was not found" ));
    }
    
    // private static async Task HandleValidationExceptionAsync(HttpContext httpContext, Exception exception)
    // {
    //     httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
    //     var validationException = exception as ValidationException;
    //     var errors = validationException?.Errors
    //         .GroupBy(e => e.PropertyName)
    //         .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
    //     await httpContext.Response.WriteAsJsonAsync(Result.FailureResponse(StatusCodes.Status400BadRequest, exception.Message, errors)); 
    // }
    
    private static async Task HandleUnauthorizedAccessExceptionAsync(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await httpContext.Response.WriteAsJsonAsync(Result.FailureResponse(StatusCodes.Status401Unauthorized, exception.Message));
    }
    
    private static async Task HandlePostgresExceptionAsync(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        const string errorMessage = "An error occurred while processing your request.";
        await httpContext.Response.WriteAsJsonAsync(Result.FailureResponse(StatusCodes.Status500InternalServerError, errorMessage));
    }

    private static async Task HandleValidationExceptionAsync(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        var validationException = exception as ValidationException;
        var errors = validationException?.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        await httpContext.Response.WriteAsJsonAsync(Result.FailureResponse(StatusCodes.Status400BadRequest, exception.Message, errors));
    }
}
