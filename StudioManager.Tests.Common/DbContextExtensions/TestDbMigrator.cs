using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Testcontainers.PostgreSql;

namespace StudioManager.Tests.Common.DbContextExtensions;

[ExcludeFromCodeCoverage]
public class TestDbMigrator<TContext>
    where TContext : DbContext
{
    private PostgreSqlContainer _postgresContainer = null!;

    public async Task StartDbAsync()
    {
        _postgresContainer = new PostgreSqlBuilder().WithDatabase($"StudioManager_Test_{Guid.NewGuid()}").Build();
        await _postgresContainer.StartAsync();
        _postgresContainer.GetConnectionString();
    }

    public async Task<string> MigrateDbAsync()
    {
        await using var context = CreateDb();
        await context.Database.MigrateAsync();
        return _postgresContainer.GetConnectionString();
    }

    public async Task ClearAsync()
    {
        await using var context = CreateDb();
        await context.Database.EnsureDeletedAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _postgresContainer.DisposeAsync();
    }

    private TContext CreateDb()
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<TContext>()
            .EnableSensitiveDataLogging();

        dbContextOptionsBuilder.UseNpgsql(_postgresContainer.GetConnectionString(),
                npgsql => { npgsql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery); })
            .UseSnakeCaseNamingConvention();

        var dbContext =
            (TContext)Activator.CreateInstance(typeof(TContext), dbContextOptionsBuilder.Options)!;
        return dbContext;
    }
}
