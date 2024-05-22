using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using NUnit.Framework;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Application.EventHandlers.Equipments;
using StudioManager.Application.Tests.Reservations.Common;
using StudioManager.Infrastructure;
using StudioManager.Notifications.Equipment;
using StudioManager.Tests.Common;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Application.Tests.EventHandlers.Equipments;

public sealed class EquipmentReturnedEventHandlerTests : IntegrationTestBase
{
    private static TestDbContextFactory<StudioManagerDbContext> _dbContextFactory = null!;
    private static FakeLogger<EquipmentReservationChangedEventHandler> _logger = null!;
    private static EquipmentReturnedEventHandler _testCandidate = null!;

    [SetUp]
    public async Task SetUpAsync()
    {
        var connectionString = await DbMigrator.MigrateDbAsync();
        _dbContextFactory = new TestDbContextFactory<StudioManagerDbContext>(connectionString);
        _logger = new FakeLogger<EquipmentReservationChangedEventHandler>();
        _testCandidate = new EquipmentReturnedEventHandler(_dbContextFactory, _logger);
    }
    
    [Test]
    public async Task should_do_nothing_when_equipment_not_found()
    {
        // Arrange
        var notification = new EquipmentReturnedEvent(Guid.NewGuid(), 1);
        
        // Act
        await _testCandidate.Handle(notification, CancellationToken.None);
        
        // Assert
        _logger.Collector.GetSnapshot().Should().Contain(x => x.Level == LogLevel.Warning);
    }
    
    [Test]
    public async Task should_change_equipment_quantity_when_equipment_not_found()
    {
        // Arrange
        Guid equipmentId;
        await using (var dbContext = await _dbContextFactory.CreateDbContextAsync(CancellationToken.None))
        { 
            var reservation = await ReservationTestHelper.AddReservationAsync(dbContext);
            equipmentId = reservation.EquipmentId;

            await dbContext.Equipments.Where(x => x.Id == equipmentId)
                .ExecuteUpdateAsync(x => x.SetProperty(y => y.Quantity, 99));
            await dbContext.SaveChangesAsync();
        }
        
        var notification = new EquipmentReturnedEvent(equipmentId, 1);
        
        // Act
        await _testCandidate.Handle(notification, CancellationToken.None);
        
        // Assert
        _logger.Collector.GetSnapshot().Should().NotContain(x => x.Level == LogLevel.Warning);
        await using (var dbContext = await _dbContextFactory.CreateDbContextAsync(CancellationToken.None))
        {
            var equipment = await dbContext.GetEquipmentAsync(equipmentId, CancellationToken.None);
            equipment!.Quantity.Should().Be(100);
        }
    }
}