using System.Diagnostics.CodeAnalysis;
using MediatR;
using StudioManager.API.Behaviours;
using StudioManager.API.Configurations;

namespace StudioManager.API;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void RegisterApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()); });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.ConfigureOAuth2(configuration);
    }
}
