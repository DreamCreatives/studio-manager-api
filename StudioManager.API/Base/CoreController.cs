using System.Diagnostics.CodeAnalysis;
using System.Net;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.ErrorMessages;

namespace StudioManager.API.Base;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json", "application/problem+json")]
[ExcludeFromCodeCoverage]
//[Authorize(Policy = BearerTokenDefaults.AuthenticationScheme)]
public abstract class CoreController(ISender sender) : ControllerBase
{
    internal async Task<IResult> SendAsync(IRequest<CommandResult> request)
    {
        var result = await sender.Send(request);

        return CreateResult(result);
    }

    internal async Task<IResult> SendAsync<T>(IRequest<QueryResult<T>> request)
    {
        var result = await sender.Send(request);

        return CreateResult(result);
    }

    private static IResult CreateResult<T>(IRequestResult<T> requestResult)
    {
        return requestResult.StatusCode switch
        {
            HttpStatusCode.OK => FromSucceededResult(requestResult),
            HttpStatusCode.NotFound => FromNotFoundResult(requestResult),
            HttpStatusCode.Conflict => FromConflictResult(requestResult),
            _ => FromUnexpectedErrorResult(requestResult)
        };
    }

    private static IResult FromSucceededResult<T>(IRequestResult<T> requestResult)
    {
        if (!requestResult.Succeeded) throw new InvalidOperationException(EX.SUCCESS_FROM_ERROR);

        return Results.Ok(requestResult.Data);
    }

    private static IResult FromNotFoundResult<T>(IRequestResult<T> requestResult)
    {
        if (requestResult.Succeeded) throw new InvalidOperationException(EX.ERROR_FROM_SUCCESS);

        return Results.Problem(
            statusCode: StatusCodes.Status404NotFound,
            detail: requestResult.Error);
    }

    private static IResult FromConflictResult<T>(IRequestResult<T> requestResult)
    {
        if (requestResult.Succeeded) throw new InvalidOperationException(EX.ERROR_FROM_SUCCESS);

        return Results.Problem(
            statusCode: StatusCodes.Status409Conflict,
            detail: requestResult.Error);
    }

    private static IResult FromUnexpectedErrorResult<T>(IRequestResult<T> requestResult)
    {
        if (requestResult.Succeeded) throw new InvalidOperationException(EX.ERROR_FROM_SUCCESS);

        return Results.Problem(
            statusCode: StatusCodes.Status500InternalServerError,
            detail: requestResult.Error);
    }
}
