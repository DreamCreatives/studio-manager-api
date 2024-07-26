using FluentAssertions;
using NUnit.Framework;
using StudioManager.API.Contracts.Reservations;
using StudioManager.Application.Reservations.GetById;
using StudioManager.Application.Tests.Reservations.Common;
using StudioManager.Domain.Entities;
using StudioManager.Infrastructure;
using StudioManager.Infrastructure.Common.Results;
using StudioManager.Tests.Common;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Application.Tests.Reservations.GetReservationByIdQueryHandlerTests;

public sealed class Handle : IntegrationTestBase
{
    private static TestDbContextFactory<StudioManagerDbContext> _dbContextFactory = null!;
    private static GetReservationByIdQueryHandler _testCandidate = null!;

    [SetUp]
    public async Task SetUpAsync()
    {
        var connectionString = await DbMigrator.MigrateDbAsync();
        _dbContextFactory = new TestDbContextFactory<StudioManagerDbContext>(connectionString);
        _testCandidate = new GetReservationByIdQueryHandler(_dbContextFactory, Mapper);
    }

    [Test]
    public async Task should_return_not_found_when_requesting_non_existing_reservation_async()
    {
        // Arrange
        var command = new GetReservationByIdQuery(Guid.NewGuid());

        // Act
        var result = await _testCandidate.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(NotFoundStatusCode);
        result.Data.Should().BeNull();
        result.Should().BeOfType<QueryResult<ReservationReadDto>>();
        result.Error.Should().NotBeNullOrWhiteSpace();
        result.Error.Should().Be($"[NOT FOUND] {nameof(Reservation)} with id '{command.Id}' does not exist");
    }

    [Test]
    public async Task should_return_success_async()
    {
        // Arrange
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var reservation =
            await ReservationTestHelper.AddReservationAsync(dbContext);

        var command = new GetReservationByIdQuery(reservation.Id);

        // Act
        var result = await _testCandidate.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(OkStatusCode);
        result.Data.Should().NotBeNull();
        result.Data.Should().BeOfType<ReservationReadDto>();
        result.Should().BeOfType<QueryResult<ReservationReadDto>>();
        result.Error.Should().BeNullOrWhiteSpace();
    }
}
