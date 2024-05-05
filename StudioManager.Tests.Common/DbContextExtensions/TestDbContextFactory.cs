using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace StudioManager.Tests.Common.DbContextExtensions;

[ExcludeFromCodeCoverage]
public class TestDbContextFactory<TContext>(string? connectionString) : IDbContextFactory<TContext>
    where TContext : DbContext
{
    private readonly string? _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    
    public TContext CreateDbContext()
    {
        throw new InvalidOperationException("This method should not be called. Use CreateDbContextAsync instead.");
    }

    public Task<TContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(CreateDbContext<TContext>());
    }
    
    private TDbContext CreateDbContext<TDbContext>() where TDbContext : DbContext
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<TDbContext>()
            .EnableSensitiveDataLogging();

        dbContextOptionsBuilder.UseNpgsql(_connectionString,
            npgsql =>
            {
                npgsql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            }).UseSnakeCaseNamingConvention();

        var dbContext = (TDbContext)Activator.CreateInstance(typeof(TDbContext), dbContextOptionsBuilder.Options)!;
        return dbContext;
    }
}