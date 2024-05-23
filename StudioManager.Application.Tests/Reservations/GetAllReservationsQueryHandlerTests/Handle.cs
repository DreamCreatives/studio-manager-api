using FluentAssertions;
using NUnit.Framework;
using StudioManager.API.Contracts.Pagination;
using StudioManager.API.Contracts.Reservations;
using StudioManager.Application.Reservations.GetAll;
using StudioManager.Application.Tests.Reservations.Common;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure;
using StudioManager.Tests.Common;
using StudioManager.Tests.Common.AutoMapperExtensions;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Application.Tests.Reservations.GetAllReservationsQueryHandlerTests;

public sealed class Handle : IntegrationTestBase
{
    private static TestDbContextFactory<StudioManagerDbContext> _dbContextFactory = null!;
    private static GetAllReservationsQueryHandler _testCandidate = null!;

    [SetUp]
    public async Task SetUpAsync()
    {
        var connectionString = await DbMigrator.MigrateDbAsync();
        _dbContextFactory = new TestDbContextFactory<StudioManagerDbContext>(connectionString);
        _testCandidate = new GetAllReservationsQueryHandler(_dbContextFactory, MappingTestHelper.Mapper);

        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var equipment = await ReservationTestHelper.AddEquipmentAsync(dbContext);
        await ReservationTestHelper.AddReservationForEquipmentAsync(dbContext, equipment.Id);
        await ReservationTestHelper.AddReservationForEquipmentAsync(dbContext, equipment.Id);
        await ReservationTestHelper.AddReservationForEquipmentAsync(dbContext, equipment.Id);
    }

    [Test]
    public async Task should_return_data_async()
    {
        // Arrange
        var query = new GetAllReservationsQuery(new ReservationFilter(), new PaginationDto());

        // Act
        var result = await _testCandidate.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Error.Should().BeNullOrWhiteSpace();
        result.Data.Should().NotBeNull();
        result.Data!.Data.Should().NotBeEmpty();
        result.Data.Data.Should().HaveCount(3);
        result.Data.Data.Should().BeOfType<List<ReservationReadDto>>();
    }
}
