﻿using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace StudioManager.Tests.Common.DbContextExtensions;

public class TestDbMigrator<TContext>
    where TContext : DbContext
{
    private PostgreSqlContainer _postgresContainer = null!;

    public async Task StartDbAsync()
    {
        _postgresContainer = new PostgreSqlBuilder().WithDatabase("testdb").Build();
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
            npgsql =>
            {
                npgsql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            }).UseSnakeCaseNamingConvention();

        var dbContext = (TContext)Activator.CreateInstance(typeof(TContext), dbContextOptionsBuilder.Options)!;
        return dbContext;
    }
}