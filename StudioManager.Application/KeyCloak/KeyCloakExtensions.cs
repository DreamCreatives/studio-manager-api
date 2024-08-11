using FS.Keycloak.RestApiClient.Model;
using StudioManager.Domain.Entities;

namespace StudioManager.Application.KeyCloak;

internal static class KeyCloakExtensions
{
    internal static void UpdateUserValues(this UserRepresentation user, User dbUser, bool verifyEmail)
    {
        user.Email = dbUser.Email;
        user.FirstName = dbUser.FirstName;
        user.LastName = dbUser.LastName;
        user.Username = $"{dbUser.FirstName}.{dbUser.LastName}";
        user.EmailVerified = verifyEmail;
        user.Enabled = true;
    }
}
