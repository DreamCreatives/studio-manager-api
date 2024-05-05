using FluentAssertions;
using NUnit.Framework;
using StudioManager.API.Contracts.EquipmentTypes;
using StudioManager.Application.EquipmentTypes.GetAll;
using StudioManager.Domain.Entities;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure;
using StudioManager.Tests.Common;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Application.Tests.EquipmentTypes.GetEquipmentTypesQueryHandlerTests;

public sealed class Handle : IntegrationTestBase
{
    private static GetEquipmentTypesQueryHandler _testCandidate = null!;
    private static TestDbContextFactory<StudioManagerDbContext> _testDbContextFactory = null!;

    [SetUp]
    public async Task SetUpAsync()
    {
        var connectionString = await DbMigrator.MigrateDbAsync();
        _testDbContextFactory = new TestDbContextFactory<StudioManagerDbContext>(connectionString);
        _testCandidate = new GetEquipmentTypesQueryHandler(_testDbContextFactory, Mapper);
    }

    [Test]
    public async Task should_return_empty_collection_when_there_is_no_data_async()
    {
        // Arrange
        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync())
        {
            await ClearTableContentsForAsync<EquipmentType>(dbContext);
        }
        
        var query = new GetEquipmentTypesQuery { Filter = new EquipmentTypeFilter() };
        
        // Act
        var result = await _testCandidate.Handle(query, Cts.Token);
        
        // Assert
        result.Should().NotBeNull();
        result.Error.Should().BeNullOrWhiteSpace();
        result.StatusCode.Should().Be(OkStatusCode);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Should().BeEmpty();
        result.Data.Should().BeOfType<List<EquipmentTypeReadDto>>();
    }
    
    [Test]
    public async Task should_return_mapped_data_async()
    {
        // Arrange
        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync())
        {
            await ClearTableContentsForAsync<EquipmentType>(dbContext);
            var equipmentTypes = Enumerable.Range(0, 5).Select(x => EquipmentType.Create(x.ToString())).ToArray();
            await AddEntitiesToTable(dbContext, equipmentTypes);
        }
        
        var query = new GetEquipmentTypesQuery { Filter = new EquipmentTypeFilter() };
        
        // Act
        var result = await _testCandidate.Handle(query, Cts.Token);
        
        // Assert
        result.Should().NotBeNull();
        result.Error.Should().BeNullOrWhiteSpace();
        result.StatusCode.Should().Be(OkStatusCode);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Should().NotBeEmpty();
        result.Data!.Count.Should().Be(5);
        result.Data.Should().BeOfType<List<EquipmentTypeReadDto>>();
    }
    
    [Test]
    public async Task should_return_mapped_and_filtered_data_async()
    {
        // Arrange
        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync())
        {
            await ClearTableContentsForAsync<EquipmentType>(dbContext);
            var equipmentTypes = Enumerable.Range(0, 5).Select(x => EquipmentType.Create(x.ToString())).ToArray();
            await AddEntitiesToTable(dbContext, equipmentTypes);
        }
        
        var query = new GetEquipmentTypesQuery { Filter = new EquipmentTypeFilter { Name = "1" } };
        
        // Act
        var result = await _testCandidate.Handle(query, Cts.Token);
        
        // Assert
        result.Should().NotBeNull();
        result.Error.Should().BeNullOrWhiteSpace();
        result.StatusCode.Should().Be(OkStatusCode);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Should().NotBeEmpty();
        result.Data!.Count.Should().Be(1);
        result.Data.Should().BeOfType<List<EquipmentTypeReadDto>>();
    }
}