using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolutionManager.Infrastructure.Common;

namespace SolutionManager.Infrastructure;

public static class DependencyInjection
{
    public static void RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConfiguration = configuration.GetSection(DatabaseConfiguration.NodeName).Get<DatabaseConfiguration>();
        
        ArgumentNullException.ThrowIfNull(databaseConfiguration, nameof(databaseConfiguration));
        
        services.RegisterDbContext<StudioManagerDbContext>(databaseConfiguration.Write);
        services.RegisterDbContext<StudioManagerReadDbContext>(databaseConfiguration.Read);
    }

    private static void RegisterDbContext<TContext>(this IServiceCollection services, DatabaseConfigurationNode connectionDetails)
        where TContext : DbContextBase
    {
        var connectionString = connectionDetails.GetConnectionString();
        
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString, nameof(connectionString));
        
        services.AddPooledDbContextFactory<TContext>(options =>
        {
            options.UseNpgsql(connectionString, opt =>
            {
                opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });

            options.UseSnakeCaseNamingConvention();
        });
    }
}