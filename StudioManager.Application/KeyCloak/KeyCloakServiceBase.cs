using System.Net;
using System.Runtime.CompilerServices;
using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.Authentication.ClientFactory;
using FS.Keycloak.RestApiClient.Authentication.Flow;
using FS.Keycloak.RestApiClient.Client;
using Microsoft.Extensions.Logging;
using StudioManager.Infrastructure.Configuration;
using ApiClientFactory = FS.Keycloak.RestApiClient.ClientFactory.ApiClientFactory;

namespace StudioManager.Application.KeyCloak;

public abstract class KeyCloakServiceBase(
    KeyCloakConfiguration keycloakConfiguration,
    ILogger<KeyCloakServiceBase> logger)
{
    private readonly ClientCredentialsFlow _clientCredentialsFlow = new()
    {
        KeycloakUrl = keycloakConfiguration.Url,
        ClientId = keycloakConfiguration.ClientId,
        ClientSecret = keycloakConfiguration.Secret,
        Realm = keycloakConfiguration.Realm
    };
    
    
    protected async Task<TResult> InvokeActionAsync<TResult>(
        Func<IUsersApiAsync, ClientCredentialsFlow, CancellationToken, Task<TResult>> asyncAction,
        Func<Exception, TResult> onException,
        CancellationToken cancellationToken = default,
        [CallerMemberName] string callerMemberName = "")
    {
        try
        {
            using var httpClient = AuthenticationHttpClientFactory.Create(_clientCredentialsFlow);

            using var usersApi = ApiClientFactory.Create<UsersApi>(httpClient);

            return await asyncAction(usersApi, _clientCredentialsFlow, cancellationToken);
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
}
