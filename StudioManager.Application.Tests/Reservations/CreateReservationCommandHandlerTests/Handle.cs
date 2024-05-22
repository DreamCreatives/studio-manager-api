using FluentAssertions;
using NUnit.Framework;
using StudioManager.API.Contracts.Reservations;
using StudioManager.Application.Reservations.Create;
using StudioManager.Application.Tests.Reservations.Common;
using StudioManager.Domain.Entities;
using StudioManager.Domain.ErrorMessages;
using StudioManager.Infrastructure;
using StudioManager.Tests.Common;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Application.Tests.Reservations.CreateReservationCommandHandlerTests;

public sealed class Handle : IntegrationTestBase
{
    private static readonly DateOnly ValidDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
    private static TestDbContextFactory<StudioManagerDbContext> _dbContextFactory = null!;
    private static CreateReservationCommandHandler _testCandidate = null!;

    [SetUp]
    public async Task SetUpAsync()
    {
        var connectionString = await DbMigrator.MigrateDbAsync();
        _dbContextFactory = new TestDbContextFactory<StudioManagerDbContext>(connectionString);
        _testCandidate = new CreateReservationCommandHandler(_dbContextFactory);
    }
    
    [Test]
    public async Task should_return_conflict_when_equipment_not_found()
    {
        // Arrange
        var command = new CreateReservationCommand(new ReservationWriteDto(ValidDate, ValidDate, 1, Guid.NewGuid()));
        
        // Act
        var result = await _testCandidate.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(ConflictStatusCode);
        result.Data.Should().BeNull();
        result.Error.Should().NotBeNullOrWhiteSpace();
        result.Error.Should().Be(DB.RESERVATION_EQUIPMENT_NOT_FOUND);
    }
    
    [Test]
    public async Task should_return_conflict_when_equipment_quantity_insufficient()
    {
        // Arrange
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var equipment = await ReservationTestHelper.AddEquipmentAsync(dbContext);
        
        var command = new CreateReservationCommand(new ReservationWriteDto(ValidDate, ValidDate, equipment.InitialQuantity + 1, equipment.Id));
        
        // Act
        var result = await _testCandidate.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(ConflictStatusCode);
        result.Data.Should().BeNull();
        result.Error.Should().NotBeNullOrWhiteSpace();
        result.Error.Should().Be(DB.RESERVATION_EQUIPMENT_QUANTITY_INSUFFICIENT);
    }

    [Test]
    public async Task should_return_error_when_other_reservations_reserved_all_equipment_async()
    {
        // Arrange
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var reservation = await ReservationTestHelper.AddReservationForDetailsAsync(dbContext, ValidDate, ValidDate.AddDays(10));
        
        var command = new CreateReservationCommand(new ReservationWriteDto(ValidDate.AddDays(1), ValidDate.AddDays(2), reservation.Quantity, reservation.EquipmentId));
        
        // Act
        var result = await _testCandidate.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(ConflictStatusCode);
        result.Data.Should().BeNull();
        result.Error.Should().NotBeNullOrWhiteSpace();
        result.Error.Should().Be(DB.RESERVATION_EQUIPMENT_USED_BY_OTHERS_IN_PERIOD);
    }
    
    [Test]
    public async Task should_return_success_when_reservation_is_valid()
    {
        // Arrange
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        await ClearTableContentsForAsync<Reservation>(dbContext);
        var equipment = await ReservationTestHelper.AddEquipmentAsync(dbContext);
        
        var command = new CreateReservationCommand(new ReservationWriteDto(ValidDate, ValidDate.AddDays(1), 1, equipment.Id));
        
        // Act
        var result = await _testCandidate.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(OkStatusCode);
        result.Data.Should().NotBeNull();
        result.Error.Should().BeNullOrWhiteSpace();
    }
}