using System.Net;

namespace StudioManager.Domain.Common.Results;

public interface IRequestResult<T>
{
    public bool Succeeded { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }
}