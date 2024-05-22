using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Infrastructure;
using StudioManager.Notifications.Equipment;

namespace StudioManager.Application.EventHandlers.Equipments;

public sealed class EquipmentReturnedEventHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory,
    ILogger<EquipmentReservationChangedEventHandler> logger)
    : INotificationHandler<EquipmentReturnedEvent>
{
    public async Task Handle(EquipmentReturnedEvent notification, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var equipment = await dbContext.GetEquipmentAsync(notification.Id, cancellationToken);
        
        if (equipment is null)
        {
            logger.LogWarning("Equipment with Id '{EquipmentId}' was not found when handling {Notification}",
                notification.Id, nameof(EquipmentReturnedEvent));
            return;
        }
        
        equipment.Reserve(0, notification.Quantity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}