using System.Diagnostics.CodeAnalysis;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StudioManager.Domain.Common;
using StudioManager.Domain.ErrorMessages;

namespace StudioManager.API.Base;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ExcludeFromCodeCoverage]
//[Authorize(Policy = "AuthorizedUser")]
public abstract class CoreController(ISender sender) : ControllerBase
{
    protected readonly ISender Sender = sender;
    
    protected static IResult FromSucceededResult<T>(QueryResult<T> queryResult)
    {
        if (!queryResult.Succeeded)
        {
            throw new InvalidOperationException(EX.SUCCESS_FROM_ERROR);
        }
        
        return Results.Ok(queryResult.Data);
    }
    
    protected static IResult FromNotFoundResult<T>(QueryResult<T> queryResult)
    {
        if (queryResult.Succeeded)
        {
            throw new InvalidOperationException(EX.ERROR_FROM_SUCCESS);
        }

        return Results.Problem(
            statusCode: StatusCodes.Status404NotFound,
            detail: queryResult.Error);
    }
    
    protected static IResult FromConflictResult<T>(QueryResult<T> queryResult)
    {
        if (queryResult.Succeeded)
        {
            throw new InvalidOperationException(EX.ERROR_FROM_SUCCESS);
        }

        return Results.Problem(
            statusCode: StatusCodes.Status409Conflict,
            detail: queryResult.Error);
    }
    
    protected static IResult FromUnexpectedErrorResult<T>(QueryResult<T> queryResult)
    {
        if (queryResult.Succeeded)
        {
            throw new InvalidOperationException(EX.ERROR_FROM_SUCCESS);
        }

        return Results.Problem(
            statusCode: StatusCodes.Status500InternalServerError,
            detail: queryResult.Error);
    }
}