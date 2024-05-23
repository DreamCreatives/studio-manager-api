using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using StudioManager.Application.Reservations.Delete;
using StudioManager.Application.Tests.Reservations.Common;
using StudioManager.Domain.Entities;
using StudioManager.Infrastructure;
using StudioManager.Tests.Common;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Application.Tests.Reservations.DeleteReservationCommandHandlerTests;

public sealed class Handle : IntegrationTestBase
{
    private static readonly DateOnly ValidDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
    private static TestDbContextFactory<StudioManagerDbContext> _dbContextFactory = null!;
    private static DeleteReservationCommandHandler _testCandidate = null!;

    [SetUp]
    public async Task SetUpAsync()
    {
        var connectionString = await DbMigrator.MigrateDbAsync();
        _dbContextFactory = new TestDbContextFactory<StudioManagerDbContext>(connectionString);
        _testCandidate = new DeleteReservationCommandHandler(_dbContextFactory);
    }

    [Test]
    public async Task should_return_not_found_when_removing_non_existing_reservation_async()
    {
        // Arrange
        var command = new DeleteReservationCommand(Guid.NewGuid());

        // Act
        var result = await _testCandidate.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(NotFoundStatusCode);
        result.Data.Should().BeNull();
        result.Error.Should().NotBeNullOrWhiteSpace();
        result.Error.Should().Be($"[NOT FOUND] {nameof(Reservation)} with id '{command.Id}' does not exist");
    }

    [Test]
    public async Task should_return_success_async()
    {
        // Arrange
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var reservation =
            await ReservationTestHelper.AddReservationForDetailsAsync(dbContext, ValidDate, ValidDate.AddDays(1), 1);

        var command = new DeleteReservationCommand(reservation.Id);

        // Act
        var result = await _testCandidate.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(OkStatusCode);
        result.Data.Should().BeNull();
        result.Error.Should().BeNullOrWhiteSpace();

        var count = await dbContext.Reservations.CountAsync();
        count.Should().Be(0);
    }
}
