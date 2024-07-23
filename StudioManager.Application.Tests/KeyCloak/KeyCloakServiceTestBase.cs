using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.Authentication.Flow;
using Microsoft.Extensions.Logging.Testing;
using StudioManager.Application.KeyCloak;

namespace StudioManager.Application.Tests.KeyCloak;

public abstract class KeyCloakServiceTestBase
{
    private static readonly ClientCredentialsFlow ClientCredentials = new()
    {
        KeycloakUrl = "http://localhost:8080",
        Realm = "realm",
        ClientId = "client_id",
        ClientSecret = "client_secret"
    };

    protected readonly Mock<IUsersApi> UsersApiMock = new();
    protected static readonly KeyCloakService TestCandidate = new(ClientCredentials, new FakeLogger<KeyCloakService>());
}
