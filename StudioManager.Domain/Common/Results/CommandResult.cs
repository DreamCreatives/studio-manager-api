using System.Diagnostics.CodeAnalysis;
using System.Net;
using StudioManager.Domain.ErrorMessages;

namespace StudioManager.Domain.Common.Results;

[ExcludeFromCodeCoverage]
public sealed class CommandResult : IRequestResult<object?>
{
    private CommandResult()
    {
    }

    public bool Succeeded { get; private init; }
    public HttpStatusCode StatusCode { get; private init; }
    public object? Data { get; private init; }
    public string? Error { get; private init; }

    public static CommandResult Success(object? data = null)
    {
        return new CommandResult
        {
            Succeeded = true,
            Data = data,
            Error = null,
            StatusCode = HttpStatusCode.OK
        };
    }

    public static CommandResult NotFound<T>(object? id = null)
    {
        var message = id is not null
            ? $"[NOT FOUND] {typeof(T).Name} with id '{id}' does not exist"
            : $"[NOT FOUND] {typeof(T).Name} does not exist";

        return new CommandResult
        {
            Succeeded = false,
            Data = default!,
            Error = message,
            StatusCode = HttpStatusCode.NotFound
        };
    }

    public static CommandResult Conflict(string error)
    {
        return new CommandResult
        {
            Succeeded = false,
            Data = default!,
            Error = error,
            StatusCode = HttpStatusCode.Conflict
        };
    }
    
    public static CommandResult Forbidden(string? error = null)
    {
        return new CommandResult
        {
            Succeeded = false,
            Data = default!,
            Error = error ?? EX.FORBIDDEN,
            StatusCode = HttpStatusCode.Forbidden
        };
    }

    public static CommandResult UnexpectedError(string error)
    {
        return new CommandResult
        {
            Succeeded = false,
            Data = default!,
            Error = error,
            StatusCode = HttpStatusCode.InternalServerError
        };
    }

    public static CommandResult UnexpectedError(Exception error)
    {
        var message = error.InnerException is not null
            ? $"[EX]: {error.InnerException.Message} {Environment.NewLine} [INNER]: {error.InnerException.Message}"
            : error.Message;

        return UnexpectedError(message);
    }
}
