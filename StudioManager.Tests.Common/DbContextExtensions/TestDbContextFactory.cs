using Microsoft.EntityFrameworkCore;

namespace StudioManager.Tests.Common.DbContextExtensions;

public class TestDbContextFactory<TContext>(string? connectionString) : IDbContextFactory<TContext>
    where TContext : DbContext
{
    private readonly string? _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

    private Exception? _exceptionToThrow;
    private bool _hasThrown;
    
    public TContext CreateDbContext()
    {
        throw new NotImplementedException("This method should not be called. Use CreateDbContextAsync instead.");
    }

    public Task<TContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
    {
        if (_hasThrown && _exceptionToThrow is not null)
        {
            _hasThrown = false;
            throw _exceptionToThrow;
        }

        return Task.FromResult(CreateDbContext<TContext>());
    }
    
    public TDbContext CreateDbContext<TDbContext>() where TDbContext : DbContext
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