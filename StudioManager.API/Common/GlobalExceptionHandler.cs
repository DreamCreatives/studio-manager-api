using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace StudioManager.API.Common;

[ExcludeFromCodeCoverage]
public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is ValidationException validationException)
        {
            return await HandleValidationExceptionAsync(httpContext, validationException, cancellationToken);
        }
        
        logger.LogError(exception, "[ERROR]: Error occurred while handling request {@Request}",exception.TargetSite?.DeclaringType?.FullName);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server error",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Detail = exception.InnerException?.Message ?? exception.Message
        };

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
    
    private static async Task<bool> HandleValidationExceptionAsync(HttpContext httpContext, ValidationException exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ValidationProblemDetails(GroupValidationErrors(exception.Errors))
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation error",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            Detail = "One or more validation errors occurred."
        };

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }

    private static Dictionary<string, string[]> GroupValidationErrors(IEnumerable<ValidationFailure> validationErrors)
    {
        var final = new Dictionary<string, string[]>();

        foreach (var error in validationErrors)
        {
            if (final.TryGetValue(error.PropertyName, out var value))
            {
                final[error.PropertyName] = value.Append(error.ErrorMessage).ToArray();
            }
            else
            {
                final[error.PropertyName] = [error.ErrorMessage];
            }
        }

        return final;
    }
}