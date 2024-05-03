using System.Net;

namespace StudioManager.Domain.Common;

public class QueryResult<T>
{
    public bool Succeeded { get; private set; }
    public HttpStatusCode StatusCode { get; private set; }
    public T? Data { get; private set; }
    public string? Error { get; private set; }
    
    public static QueryResult<T> Success(T data)
    {
        return new QueryResult<T>
        {
            Succeeded = true,
            Data = data,
            Error = null
        };
    }
    
    public static QueryResult<T> NotFound(string error)
    {
        return new QueryResult<T>
        {
            Succeeded = false,
            Data = default!,
            Error = error,
            StatusCode = HttpStatusCode.NotFound
        };
    }
    
    public static QueryResult<T> Conflict(string error)
    {
        return new QueryResult<T>
        {
            Succeeded = false,
            Data = default!,
            Error = error,
            StatusCode = HttpStatusCode.Conflict
        };
    }
    
    public static QueryResult<T> UnexpectedError(string error)
    {
        return new QueryResult<T>
        {
            Succeeded = false,
            Data = default!,
            Error = error,
            StatusCode = HttpStatusCode.InternalServerError
        };
    }
}

public class QueryResult
{
    public static QueryResult<T> Success<T>(T data)
    {
        return QueryResult<T>.Success(data);
    }
}