using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using FluentValidation;
using MediatR;

namespace StudioManager.API.Behaviours;

[ExcludeFromCodeCoverage]
public class RequestLoggingBehavior<TRequest, TResponse>(
    ILogger<RequestLoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        try
        {
            var requestBody = JsonSerializer.Serialize(request);
            logger.LogInformation("[START]: {@RequestName}, Start Time: {@DateTimeUtc}, Request Body: {@RequestBody}",
                typeof(TRequest).Name, DateTime.UtcNow, requestBody);
            var result = await next();

            return result;
        }
        catch (Exception e) when (e is not ValidationException)
        {
            logger.LogError(e, "[ERROR]: {@RequestName}",
                typeof(TRequest).Name);
            throw;
        }
        finally
        {
            stopWatch.Stop();
            var elapsed = stopWatch.ElapsedMilliseconds;
            logger.LogInformation("[END]: {@RequestName}, Elapsed Time: {@Elapsed} ms, End Time: {@DateTimeUtc}",
                typeof(TRequest).Name, elapsed, DateTime.UtcNow);
        }
    }
}