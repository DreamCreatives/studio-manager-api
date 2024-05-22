using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Infrastructure;
using StudioManager.Notifications.Equipment;

namespace StudioManager.Application.EventHandlers.Equipments;

public sealed class EquipmentReservationChangedEventHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory,
    ILogger<EquipmentReservationChangedEventHandler> logger)
    : INotificationHandler<EquipmentReservationChangedEvent>
{
    public async Task Handle(EquipmentReservationChangedEvent notification, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var equipment = await dbContext.GetEquipmentAsync(notification.EquipmentId, cancellationToken);
        
        if (equipment is null)
        {
            logger.LogWarning("Equipment with Id '{EquipmentId}' was not found when handling {Notification}",
                notification.EquipmentId, nameof(EquipmentReservationChangedEvent));
            return;
        }
        
        equipment.Reserve(notification.Quantity, notification.InitialQuantity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}