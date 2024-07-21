using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.Authentication.Flow;
using FS.Keycloak.RestApiClient.Model;
using Microsoft.Extensions.Logging;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Entities;

namespace StudioManager.Application.KeyCloak;

public sealed class KeyCloakService(ClientCredentialsFlow clientCredentials, ILogger<KeyCloakService> logger)
    : KeyCloakServiceBase(clientCredentials, logger), IKeyCloakService
{
    private const string EnvironmentVariable = "ASPNETCORE_ENVIRONMENT";
    private const string LocalEnvironment = "Local";
    private const string DevelopmentEnvironment = "Development";
    private static readonly string CurrentEnvironment = Environment.GetEnvironmentVariable(EnvironmentVariable) ?? DevelopmentEnvironment;
    private static readonly bool IsLocalEnvironment = CurrentEnvironment.Equals(LocalEnvironment, StringComparison.InvariantCultureIgnoreCase); // TODO: Maybe I should refactor this
    
    public async Task AddUserAsync(User user, CancellationToken cancellationToken = default)
    {
        var identityUser = new UserRepresentation
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Username = $"{user.FirstName}.{user.LastName}",
            EmailVerified = IsLocalEnvironment,
            Enabled = true,
            Credentials = [
                new CredentialRepresentation
                {
                    Type = "password",
                    Value = Convert.ToBase64String(user.Id.ToByteArray()),
                    Temporary = IsLocalEnvironment
                }]
        };
        
        await InvokeActionAsync(AsyncAction, cancellationToken);
        
        return;
        
        async Task AsyncAction(IUsersApiAsync usersApi,ClientCredentialsFlow credentials, CancellationToken ct)
        {
            await usersApi.PostUsersAsync(credentials.Realm, identityUser, ct);
        }
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
        
        identityUser.UpdateUserValues(user, IsLocalEnvironment);
        
        return await InvokeActionAsync(AsyncAction, OnException, cancellationToken);
        
        async Task<CommandResult> AsyncAction(IUsersApiAsync usersApi, ClientCredentialsFlow credentials, CancellationToken ct)
        {
            await usersApi.PutUsersByUserIdAsync(credentials.Realm, user.KeycloakId, identityUser, ct);

            return CommandResult.Success();
        }
        
        CommandResult OnException(Exception e) => CommandResult.UnexpectedError(e.InnerException?.Message ?? e.Message);
    }
}
