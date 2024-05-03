using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using MediatR;
using StudioManager.Domain.Common;

namespace StudioManager.API.Behaviours;

[ExcludeFromCodeCoverage]
public sealed class RequestLoggingBehavior<TRequest, TResponse>(
    ILogger<RequestLoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : QueryResult<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        try
        {
            var requestBody = JsonSerializer.Serialize(request);
            logger.LogInformation("[START]: handling request {@RequestName}, Start Time: {@DateTimeUtc}, Request Body: {@RequestBody}",
                typeof(TRequest).Name, DateTime.UtcNow, requestBody);
            var result = await next();
            stopWatch.Stop();
            var elapsed = stopWatch.ElapsedMilliseconds;
            logger.LogInformation("[END]: handling request {@RequestName}, Elapsed Time: {@Elapsed} MS, End Time: {@DateTimeUtc}",
                typeof(TRequest).Name, elapsed, DateTime.UtcNow);

            return result;
        }
        catch (Exception e)
        {
            stopWatch.Stop();
            var elapsed = stopWatch.ElapsedMilliseconds;
            logger.LogError(e, "[ERROR]: handling request {@RequestName}, Elapsed Time: {@Elapsed} MS, End Time: {@DateTimeUtc}",
                typeof(TRequest).Name, elapsed, DateTime.UtcNow);
            throw;
        }
    }
}