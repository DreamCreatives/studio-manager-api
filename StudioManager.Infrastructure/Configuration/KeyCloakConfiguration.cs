using System.Diagnostics.CodeAnalysis;

namespace StudioManager.Infrastructure.Configuration;

[ExcludeFromCodeCoverage]
public class KeyCloakConfiguration
{
    public const string SectionName = "Authentication";
    public const string HttpClientName = "KeyCloakClient";
    public string Secret { get; init; } = string.Empty;
    public string ClientId { get; init; } = string.Empty;
    public string Authority { get; init; } = string.Empty;
    public string AuthEndpoint { get; init; } = string.Empty;
    public string TokenUrl { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public string Realm { get; init; } = string.Empty;

    public Uri GetTokenUri()
    {
        return GetUri(TokenUrl);
    }
    
    public Uri GetAuthEndpointUri()
    {
        return GetUri(AuthEndpoint);
    }
    
    private Uri GetUri(string url)
    {
        if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            return uri;
        }
        
        throw new ArgumentException("Invalid URL", nameof(url));
    }
}
