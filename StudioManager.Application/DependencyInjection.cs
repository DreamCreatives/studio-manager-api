using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace StudioManager.Application;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void RegisterApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(opt => { opt.AddMaps(AppDomain.CurrentDomain.GetAssemblies()); });

        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies(), includeInternalTypes: true);
    }
}
