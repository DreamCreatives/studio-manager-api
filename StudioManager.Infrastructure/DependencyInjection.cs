using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudioManager.Infrastructure.Common;

namespace StudioManager.Infrastructure;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConfiguration =
            configuration.GetSection(DatabaseConfiguration.NodeName).Get<DatabaseConfiguration>();

        ArgumentNullException.ThrowIfNull(databaseConfiguration, nameof(databaseConfiguration));

        services.RegisterPooledDbContext<StudioManagerDbContext>(databaseConfiguration.Write);
    }

    private static void RegisterPooledDbContext<TContext>(this IServiceCollection services,
        DatabaseConfigurationNode connectionDetails)
        where TContext : DbContextBase
    {
        var connectionString = connectionDetails.GetConnectionString();

        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString, nameof(connectionString));

        services.AddPooledDbContextFactory<TContext>(options =>
        {
            options.UseNpgsql(connectionString,
                opt => { opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery); });

            options.UseSnakeCaseNamingConvention();
        });
    }
}
