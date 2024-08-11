using FS.Keycloak.RestApiClient.Api;
using Microsoft.Extensions.Logging.Testing;
using StudioManager.Application.KeyCloak;
using StudioManager.Infrastructure.Configuration;

namespace StudioManager.Application.Tests.KeyCloak;

public abstract class KeyCloakServiceTestBase
{
    private static readonly KeyCloakConfiguration ClientCredentials = new()
    {
        Url = "http://localhost:8080",
        Realm = "realm",
        ClientId = "client_id",
        Secret = "client_secret",
        SaveUserAsEnabled = true,
        EmailVerified = true
    };

    protected readonly Mock<IUsersApi> UsersApiMock = new();
    protected static readonly KeyCloakService TestCandidate = new(ClientCredentials, new FakeLogger<KeyCloakService>());
}
