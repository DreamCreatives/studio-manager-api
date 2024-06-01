using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using StudioManager.Infrastructure.Configuration;

namespace StudioManager.API.Configurations;

[ExcludeFromCodeCoverage]
public static class AuthenticationConfiguration
{
    public static void ConfigureOAuth2(this IServiceCollection services, IConfiguration configuration)
    {
        var authConfig = configuration.GetRequiredSection(KeyCloakConfiguration.SectionName).Get<KeyCloakConfiguration>();
        
        ArgumentNullException.ThrowIfNull(authConfig, nameof(authConfig));
        
        services.AddAuthentication(opt => 
            {
                opt.DefaultScheme = BearerTokenDefaults.AuthenticationScheme;
                opt.DefaultAuthenticateScheme = BearerTokenDefaults.AuthenticationScheme;
                opt.DefaultSignInScheme = BearerTokenDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(BearerTokenDefaults.AuthenticationScheme, opt => 
            {
                opt.Authority = authConfig.Authority;
                opt.Audience = authConfig.ClientId;
                opt.RequireHttpsMetadata = false; // TODO: Change to true in production
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authConfig.Authority,
                    AuthenticationType = BearerTokenDefaults.AuthenticationScheme,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateAudience = false
                };
            })
            .AddOpenIdConnect("keycloak", opt => 
            {
                opt.Authority = authConfig.Authority;
                opt.ClientId = authConfig.ClientId;
                opt.ClientSecret = authConfig.Secret;
                opt.ResponseType = OpenIdConnectResponseType.IdTokenToken;
                opt.GetClaimsFromUserInfoEndpoint = true;
                opt.SaveTokens = true;
                opt.Scope.Add("openid");
                opt.RequireHttpsMetadata = false; // TODO: Change to true in production
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authConfig.Authority,
                    AuthenticationType = BearerTokenDefaults.AuthenticationScheme,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateAudience = false
                }; 
            });
        
        services.AddAuthorizationBuilder()
            .AddPolicy(BearerTokenDefaults.AuthenticationScheme, policy => 
            {
                policy.AddAuthenticationSchemes(BearerTokenDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
            });
    }
}
