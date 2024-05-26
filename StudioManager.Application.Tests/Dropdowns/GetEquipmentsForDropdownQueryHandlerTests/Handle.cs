using FluentAssertions;
using NUnit.Framework;
using StudioManager.API.Contracts.Common;
using StudioManager.Application.Dropdowns.Equipments;
using StudioManager.Domain.Entities;
using StudioManager.Infrastructure;
using StudioManager.Tests.Common;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Application.Tests.Dropdowns.GetEquipmentsForDropdownQueryHandlerTests;

public sealed class Handle : IntegrationTestBase
{
    private static GetEquipmentsForDropdownQueryHandler _testCandidate = null!;
    private static TestDbContextFactory<StudioManagerDbContext> _testDbContextFactory = null!;

    [SetUp]
    public async Task SetUpAsync()
    {
        var connectionString = await DbMigrator.MigrateDbAsync();
        _testDbContextFactory = new TestDbContextFactory<StudioManagerDbContext>(connectionString);
        _testCandidate = new GetEquipmentsForDropdownQueryHandler(_testDbContextFactory, Mapper);
    }
    
    [Test]
    public async Task should_return_mapped_data_with_pagination_async()
    {
        // Arrange
        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await ClearTableContentsForAsync<EquipmentType>(dbContext);
            await ClearTableContentsForAsync<Equipment>(dbContext);
            var equipmentType = EquipmentType.Create("Test-Equipment-Type");
            await AddEntitiesToTable(dbContext, equipmentType);
            var equipments = Enumerable.Range(0, 5)
                .Select(x =>
                    Equipment.Create(x.ToString(), equipmentType.Id, x))
                .ToArray();
            await AddEntitiesToTable(dbContext, equipments);
        }

        var query = new GetEquipmentsForDropdownQuery(null);

        // Act
        var result = await _testCandidate.Handle(query, Cts.Token);

        // Assert
        result.Should().NotBeNull();
        result.Error.Should().BeNullOrWhiteSpace();
        result.StatusCode.Should().Be(OkStatusCode);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeEmpty();
        result.Data.Should().BeOfType<List<NamedBaseDto>>();
    }
}
