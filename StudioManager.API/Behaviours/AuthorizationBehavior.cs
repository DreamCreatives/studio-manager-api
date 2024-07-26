using System.Diagnostics.CodeAnalysis;
using System.Net;
using MediatR;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.API.Behaviours;

[ExcludeFromCodeCoverage]
public sealed class AuthorizationBehavior<TRequest, TResponse>(
    IAuthorizationHandler<TRequest> authorizationHandler)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
    where TResponse : IRequestResult<CommandResult>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authResult = await authorizationHandler.AuthorizeAsync(request, cancellationToken);

        return authResult is { Succeeded: false, StatusCode: HttpStatusCode.Forbidden }
            ?  (TResponse) (object) authResult
            : await next();
    }
}
