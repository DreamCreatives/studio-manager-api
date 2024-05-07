using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using StudioManager.API.Contracts.Equipments;
using StudioManager.API.Contracts.Pagination;
using StudioManager.Application.Equipments.GetAll;
using StudioManager.Domain.Entities;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure;
using StudioManager.Tests.Common;
using StudioManager.Tests.Common.AutoMapperExtensions;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Application.Tests.Equipments.GetAllEquipmentsQueryHandlerTests;

public sealed class Handle : IntegrationTestBase
{
    private static GetAllEquipmentsQueryHandler _testCandidate = null!;
    private static IMapper _mapper = null!;
    private static TestDbContextFactory<StudioManagerDbContext> _testDbContextFactory = null!;

    [SetUp]
    public async Task SetUpAsync()
    {
        _mapper = MappingTestHelper.Mapper;
        var connectionString = await DbMigrator.MigrateDbAsync();
        _testDbContextFactory = new TestDbContextFactory<StudioManagerDbContext>(connectionString);
        _testCandidate = new GetAllEquipmentsQueryHandler(_mapper, _testDbContextFactory);
    }

    [Test]
    public async Task should_return_empty_data_with_pagination_async()
    {
        // Arrange
        var query = new GetAllEquipmentsQuery(new EquipmentFilter(), PaginationDto.Default());
        
        // Act
        var result = await _testCandidate.Handle(query, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Error.Should().BeNullOrWhiteSpace();
        result.StatusCode.Should().Be(OkStatusCode);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Should().BeOfType<PagingResultDto<EquipmentReadDto>>();
        result.Data!.Data.Should().BeEmpty();
        result.Data.Data.Should().BeOfType<List<EquipmentReadDto>>();
    }
    
    [Test]
    public async Task should_return_mapped_data_with_pagination_async()
    {
        // Arrange
        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync())
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
        var query = new GetAllEquipmentsQuery(new EquipmentFilter(), PaginationDto.Default());
        
        // Act
        var result = await _testCandidate.Handle(query, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Error.Should().BeNullOrWhiteSpace();
        result.StatusCode.Should().Be(OkStatusCode);
        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Should().BeOfType<PagingResultDto<EquipmentReadDto>>();
        result.Data!.Pagination.Should().NotBeNull();
        result.Data.Pagination.Limit.Should().Be(25);
        result.Data.Pagination.Page.Should().Be(1);
        result.Data.Pagination.Total.Should().Be(5);
        result.Data.Data.Should().NotBeEmpty();
        result.Data.Data.Should().HaveCount(5);
        result.Data.Data.Should().BeOfType<List<EquipmentReadDto>>();
    }
}