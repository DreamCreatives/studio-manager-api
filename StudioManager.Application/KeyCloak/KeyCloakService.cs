using System.Net;
using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.Authentication.Flow;
using FS.Keycloak.RestApiClient.Client;
using FS.Keycloak.RestApiClient.Model;
using Microsoft.Extensions.Logging;
using StudioManager.Domain.Entities;
using StudioManager.Infrastructure.Common.Results;
using StudioManager.Infrastructure.Configuration;

namespace StudioManager.Application.KeyCloak;

public sealed class KeyCloakService(KeyCloakConfiguration keycloakConfiguration, ILogger<KeyCloakService> logger)
    : KeyCloakServiceBase(keycloakConfiguration, logger), IKeyCloakService
{
    private readonly KeyCloakConfiguration _keycloakConfiguration = keycloakConfiguration;

    public async Task<CommandResult> AddUserAsync(User user, CancellationToken cancellationToken = default)
    {
        var identityUser = new UserRepresentation
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Username = $"{user.FirstName}.{user.LastName}",
            EmailVerified = _keycloakConfiguration.EmailVerified,
            Enabled = _keycloakConfiguration.SaveUserAsEnabled,
            Credentials = [
                new CredentialRepresentation
                {
                    Type = "password",
                    Value = Convert.ToBase64String(user.Id.ToByteArray()),
                    Temporary = _keycloakConfiguration.SaveUserAsEnabled
                }]
        };
        
        return await InvokeActionAsync(AsyncAction, OnException, cancellationToken);
        
        async Task<CommandResult> AsyncAction(IUsersApiAsync usersApi, ClientCredentialsFlow credentials, CancellationToken ct)
        {
            await usersApi.PostUsersAsync(credentials.Realm, identityUser, ct);

            return CommandResult.Success();
        }

        CommandResult OnException(Exception e) => BaseOnException(e, user.Email);
    }

    public async Task<UserRepresentation?> GetIdentityUserByEmail(string email, CancellationToken cancellationToken = default)
    {
        return await InvokeActionAsync(AsyncAction, OnException, cancellationToken);
        
        async Task<UserRepresentation?> AsyncAction(IUsersApiAsync usersApi,ClientCredentialsFlow credentials, CancellationToken ct)
        {
            var users = await usersApi.GetUsersAsync(credentials.Realm, email: email, cancellationToken: cancellationToken);

            return users.FirstOrDefault(x => x.Email == email);
        }
        
        UserRepresentation? OnException(Exception e) => null;
    }
    
    public async Task<UserRepresentation?> GetIdentityUserById(string keycloakId, CancellationToken cancellationToken = default)
    {
        return await InvokeActionAsync(AsyncAction, OnException, cancellationToken);
        
        async Task<UserRepresentation?> AsyncAction(IUsersApiAsync usersApi,ClientCredentialsFlow credentials, CancellationToken ct)
        {
            return await usersApi.GetUsersByUserIdAsync(credentials.Realm, keycloakId, null, cancellationToken);
        }
        
        UserRepresentation? OnException(Exception e) => null;
    }
    
    public async Task<CommandResult> UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        var identityUser = await GetIdentityUserById(user.KeycloakId, cancellationToken);

        if (identityUser is null)
        {
            return CommandResult.NotFound<UserRepresentation>(user.KeycloakId);
        }
        
        identityUser.UpdateUserValues(user, _keycloakConfiguration.SaveUserAsEnabled);
        
        return await InvokeActionAsync(AsyncAction, OnException, cancellationToken);
        
        async Task<CommandResult> AsyncAction(IUsersApiAsync usersApi, ClientCredentialsFlow credentials, CancellationToken ct)
        {
            await usersApi.PutUsersByUserIdAsync(credentials.Realm, user.KeycloakId, identityUser, ct);

            return CommandResult.Success();
        }

        CommandResult OnException(Exception e) => BaseOnException(e, user.Email);
    }

    public async Task<CommandResult> RemoveUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await InvokeActionAsync(AsyncAction, OnException, cancellationToken);
        
        async Task<CommandResult> AsyncAction(IUsersApiAsync usersApi, ClientCredentialsFlow credentials, CancellationToken ct)
        {
            await usersApi.DeleteUsersByUserIdAsync(credentials.Realm, userId, ct);

            return CommandResult.Success();
        }

        CommandResult OnException(Exception e) => BaseOnException(e, userId);
    }
    
    private static CommandResult BaseOnException(Exception e, string identifier)
    {
        return e switch
        {
            ApiException { ErrorCode: (int)HttpStatusCode.NotFound } => CommandResult.NotFound<UserRepresentation>(identifier),
            ApiException { ErrorCode: (int)HttpStatusCode.Conflict } ae => CommandResult.Conflict(ae.InnerException?.Message ?? ae.Message),
            _ => CommandResult.UnexpectedError(e.InnerException?.Message ?? e.Message)
        };
    }
    
}
