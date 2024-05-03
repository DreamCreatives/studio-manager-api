using System.Net;

namespace StudioManager.Domain.Common;

public sealed class CommandResult
{
    public bool Succeeded { get; private set; }
    public HttpStatusCode StatusCode { get; private set; }
    public object? Data { get; private set; }
    public string? Error { get; private set; }
    
    public static CommandResult Success(object? data)
    {
        return new CommandResult
        {
            Succeeded = true,
            Data = data,
            Error = null
        };
    }
    
    public static CommandResult NotFound(string error)
    {
        return new CommandResult
        {
            Succeeded = false,
            Data = default!,
            Error = error,
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
}