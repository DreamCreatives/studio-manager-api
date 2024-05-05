using System.Net;
using AutoMapper;
using NUnit.Framework;
using StudioManager.Infrastructure;
using StudioManager.Tests.Common.AutoMapperExtensions;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Tests.Common;

public abstract class IntegrationTestBase
{
    protected static readonly CancellationTokenSource Cts = new();
    protected static readonly IMapper Mapper = MappingTestHelper.Mapper;
    
    
    protected const HttpStatusCode OkStatusCode = HttpStatusCode.OK;
    protected const HttpStatusCode ConflictStatusCode = HttpStatusCode.Conflict;
    protected const HttpStatusCode NotFoundStatusCode = HttpStatusCode.NotFound;
    protected const HttpStatusCode UnexpectedErrorStatusCode = HttpStatusCode.InternalServerError;

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
}