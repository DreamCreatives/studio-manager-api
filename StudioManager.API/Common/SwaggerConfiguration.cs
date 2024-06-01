using System.Diagnostics.CodeAnalysis;
using Asp.Versioning;
using Microsoft.OpenApi.Models;
using StudioManager.Infrastructure.Configuration;

namespace StudioManager.API.Common;

[ExcludeFromCodeCoverage]
public static class SwaggerConfiguration
{
    public static void ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("x-api-version"));
            })
            .AddMvc()
            .AddApiExplorer(x =>
            {
                x.GroupNameFormat = "'v'VVV";
                x.SubstituteApiVersionInUrl = true;
            });
        
        var authConfig = configuration.GetRequiredSection(KeyCloakConfiguration.SectionName).Get<KeyCloakConfiguration>();
        
        ArgumentNullException.ThrowIfNull(authConfig, nameof(authConfig));

        services.AddSwaggerGen(opt =>
        {
            opt.SupportNonNullableReferenceTypes();
            
            opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                In = ParameterLocation.Header,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        TokenUrl = authConfig.GetTokenUri(),
                        AuthorizationUrl = authConfig.GetAuthEndpointUri(),
                        Scopes = new Dictionary<string, string>()
                    }
                },
            });
            
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        services.ConfigureOptions<NamedSwaggerGenOptions>();
    }
}
