using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace StudioManager.API.Common;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtension
{
    public static void AddBehavior(this IServiceCollection services, Type behavior)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), behavior);
    }
}