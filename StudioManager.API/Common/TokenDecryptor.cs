using System.Security.Claims;
using StudioManager.Application.Common;

namespace StudioManager.API.Common;

public sealed class TokenDecryptor(IHttpContextAccessor httpContextAccessor) : ITokenDecryptor
{
    private const string ClientIdKey = "ezp";

    private string? _userId;
    private string? _clientId;

    public string? UserId => GetUserId();
    public string? ClientId => GetClientId();
    
    private string? GetUserId()
    {
        GetClaimValue(ClaimTypes.NameIdentifier, ref _userId);
        return _userId;
    }
    
    private string? GetClientId()
    {
        GetClaimValue(ClientIdKey, ref _clientId);
        return _clientId;
    }

    private void GetClaimValue(string? claim, ref string? value)
    {
        if (value is not null)
        {
            return;
        }

        var userData = httpContextAccessor.HttpContext?.User.Identity;
        
        if (userData is { IsAuthenticated: true })
        {
            var claimValue = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type.Equals(claim, StringComparison.InvariantCultureIgnoreCase));
            value = claimValue?.Value;
        }
    }
}
