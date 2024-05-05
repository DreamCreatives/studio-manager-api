using System.Net;

namespace StudioManager.Domain.Common.Results;

public sealed class CommandResult : IRequestResult<object?>
{
    public bool Succeeded { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public object? Data { get; set; }
    public string? Error { get; init; }
    
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