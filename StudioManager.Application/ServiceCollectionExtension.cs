using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using FS.Keycloak.RestApiClient.Authentication.Flow;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StudioManager.Application.Common;
using StudioManager.Application.KeyCloak;
using StudioManager.Domain.Common.Results;
using StudioManager.Infrastructure.Configuration;

namespace StudioManager.Application;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtension
{
    public static void RegisterApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(opt => { opt.AddMaps(AppDomain.CurrentDomain.GetAssemblies()); });

        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies(), includeInternalTypes: true);
        
        services.RegisterApplicationServices<IApplicationService>();
        services.RegisterAuthorizationHandlers();
        services.RegisterKeyCloakService(configuration);
    }
    
    private static void RegisterApplicationServices<TService>(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(classes => classes.AssignableTo<TService>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());
    }

    private static void RegisterAuthorizationHandlers(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(classes => classes.AssignableTo(typeof(IAuthorizationHandler<>)))
            .AsImplementedInterfaces()
            .WithSingletonLifetime());
    }

    private static void RegisterKeyCloakService(this IServiceCollection services, IConfiguration configuration)
    {
        var authOptions = configuration.GetRequiredSection(KeyCloakConfiguration.SectionName).Get<KeyCloakConfiguration>();
    
        ArgumentNullException.ThrowIfNull(authOptions, nameof(authOptions));

        var credentials = new ClientCredentialsFlow
        {
            KeycloakUrl = authOptions.Url,
            ClientId = authOptions.ClientId,
            ClientSecret = authOptions.Secret,
            Realm = authOptions.Realm
        };
        
        services.AddSingleton<IKeyCloakService>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<KeyCloakService>>();
            return new KeyCloakService(credentials, logger);
        });
    }
}
