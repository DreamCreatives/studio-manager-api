using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using StudioManager.Application.Common;
using StudioManager.Domain.Common.Results;

namespace StudioManager.Application;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void RegisterApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(opt => { opt.AddMaps(AppDomain.CurrentDomain.GetAssemblies()); });

        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies(), includeInternalTypes: true);
        
        services.RegisterApplicationServices();
        services.RegisterAuthorizationHandlers();
    }
    
    private static void RegisterApplicationServices(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(classes => classes.AssignableTo<IApplicationService>())
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
}
