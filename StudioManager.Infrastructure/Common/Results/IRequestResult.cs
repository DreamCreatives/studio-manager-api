using System.Net;
using System.Text.Json.Serialization;

namespace StudioManager.Infrastructure.Common.Results;

public interface IRequestResult<out T>
{
    public bool Succeeded { get; }
    
    [JsonIgnore]
    public HttpStatusCode StatusCode { get; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Error { get; }
}
