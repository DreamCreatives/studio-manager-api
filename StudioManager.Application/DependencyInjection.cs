using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace StudioManager.Application;

public static class DependencyInjection
{
    public static void RegisterApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(opt =>
        {
            opt.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
        });
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        });

        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
    }
}