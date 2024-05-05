using System.Diagnostics.CodeAnalysis;
using MediatR;
using StudioManager.API.Behaviours;

namespace StudioManager.API;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void RegisterApi(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        });
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLoggingBehavior<,>));
    }
}