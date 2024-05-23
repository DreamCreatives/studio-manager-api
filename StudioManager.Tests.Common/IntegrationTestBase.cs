using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using StudioManager.Infrastructure;
using StudioManager.Infrastructure.Common;
using StudioManager.Tests.Common.AutoMapperExtensions;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Tests.Common;

[ExcludeFromCodeCoverage]
public abstract class IntegrationTestBase
{
    protected const HttpStatusCode OkStatusCode = HttpStatusCode.OK;
    protected const HttpStatusCode ConflictStatusCode = HttpStatusCode.Conflict;
    protected const HttpStatusCode NotFoundStatusCode = HttpStatusCode.NotFound;
    protected const HttpStatusCode UnexpectedErrorStatusCode = HttpStatusCode.InternalServerError;
    protected static readonly CancellationTokenSource Cts = new();
    protected static readonly IMapper Mapper = MappingTestHelper.Mapper;

    protected TestDbMigrator<StudioManagerDbContext> DbMigrator { get; private set; } = null!;

    [OneTimeSetUp]
    public async Task SetupContainersAsync()
    {
        DbMigrator = new TestDbMigrator<StudioManagerDbContext>();
        await DbMigrator.StartDbAsync();
    }

    [OneTimeTearDown]
    public async Task DisposeContainersAsync()
    {
        await DbMigrator.ClearAsync();
        await DbMigrator.DisposeAsync();
    }

    protected static async Task ClearTableContentsForAsync<T>(
        DbContextBase db,
        Expression<Func<T, bool>>? filter = null)
        where T : class
    {
        if (filter is not null)
            await db.Set<T>().Where(filter).ExecuteDeleteAsync(Cts.Token);
        else
            await db.Set<T>().ExecuteDeleteAsync(Cts.Token);

        await db.SaveChangesAsync();
    }

    protected static async Task AddEntitiesToTable<T>(
        DbContextBase db,
        params T[] entities)
        where T : class
    {
        await db.Set<T>().AddRangeAsync(entities, Cts.Token);
        await db.SaveChangesAsync(Cts.Token);
    }
}
