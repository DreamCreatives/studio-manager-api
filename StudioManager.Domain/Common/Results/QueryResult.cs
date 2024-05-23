using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace StudioManager.Domain.Common.Results;

[ExcludeFromCodeCoverage]
public class QueryResult<T> : IRequestResult<T>
{
    public bool Succeeded { get; private init; }
    public HttpStatusCode StatusCode { get; private init; }
    public T? Data { get; private init; }
    public string? Error { get; private init; }

    public static QueryResult<T> Success(T data)
    {
        return new QueryResult<T>
        {
            Succeeded = true,
            Data = data,
            Error = null,
            StatusCode = HttpStatusCode.OK
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
