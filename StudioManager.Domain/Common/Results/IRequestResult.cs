using System.Net;

namespace StudioManager.Domain.Common.Results;

public interface IRequestResult<out T>
{
    public bool Succeeded { get; }
    public HttpStatusCode StatusCode { get; }
    public T? Data { get; }
    public string? Error { get; }
}