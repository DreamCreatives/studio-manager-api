using System.Net;
using System.Runtime.CompilerServices;
using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.Authentication.ClientFactory;
using FS.Keycloak.RestApiClient.Authentication.Flow;
using FS.Keycloak.RestApiClient.Client;
using Microsoft.Extensions.Logging;
using ApiClientFactory = FS.Keycloak.RestApiClient.ClientFactory.ApiClientFactory;

namespace StudioManager.Application.KeyCloak;

public abstract class KeyCloakServiceBase(
    ClientCredentialsFlow clientCredentials,
    ILogger<KeyCloakServiceBase> logger)
{
    protected async Task<TResult> InvokeActionAsync<TResult>(
        Func<IUsersApiAsync, ClientCredentialsFlow, CancellationToken, Task<TResult>> asyncAction,
        Func<Exception, TResult> onException,
        CancellationToken cancellationToken = default,
        [CallerMemberName] string callerMemberName = "")
    {
        try
        {
            using var httpClient = AuthenticationHttpClientFactory.Create(clientCredentials);

            using var usersApi = ApiClientFactory.Create<UsersApi>(httpClient);

            return await asyncAction(usersApi, clientCredentials, cancellationToken);
        }
        catch (ApiException e) when (e.ErrorCode == (int)HttpStatusCode.NotFound)
        {
            logger.LogError(e, "Not found. Error executing method {method}  Error: {Error}", callerMemberName, e.InnerException?.Message ?? e.Message);
            return onException(e);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error executing method {method}  Error: {Error}", callerMemberName, e.InnerException?.Message ?? e.Message);
            return onException(e);
        }
    }

    protected async Task InvokeActionAsync(
        Func<IUsersApiAsync, ClientCredentialsFlow, CancellationToken, Task> asyncAction,
        CancellationToken cancellationToken = default,
        [CallerMemberName] string callerMemberName = "")
    {
        try
        {
            using var httpClient = AuthenticationHttpClientFactory.Create(clientCredentials);

            using var usersApi = ApiClientFactory.Create<UsersApi>(httpClient);
            
            await asyncAction(usersApi, clientCredentials, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error executing method {method}  Error: {Error}", callerMemberName, e.InnerException?.Message ?? e.Message);
        }
    }
}
