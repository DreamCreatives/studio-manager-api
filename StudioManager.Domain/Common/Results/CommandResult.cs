using System.Net;

namespace StudioManager.Domain.Common.Results;

public sealed class CommandResult : IRequestResult<object?>
{
    public bool Succeeded { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public object? Data { get; set; }
    public string? Error { get; set; }
    
    public static CommandResult Success(object? data = null)
    {
        return new CommandResult
        {
            Succeeded = true,
            Data = data,
            Error = null
        };
    }
    
    public static CommandResult NotFound<T>(object? id = null)
    {
        return new CommandResult
        {
            Succeeded = false,
            Data = default!,
            Error = $"[NOT FOUND] {typeof(T).Name} with id {id} does not exist",
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