using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Infrastructure;
using StudioManager.Notifications.Equipment;

namespace StudioManager.Application.EventHandlers.Equipments;

public sealed class EquipmentReservedEventHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory,
    ILogger<EquipmentReservationChangedEventHandler> logger)
    : INotificationHandler<EquipmentReservedEvent>
{
    public async Task Handle(EquipmentReservedEvent notification, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var equipment = await dbContext.GetEquipmentAsync(notification.Id, cancellationToken);

        if (equipment is null)
        {
            logger.LogWarning("Equipment with Id '{EquipmentId}' was not found when handling {Notification}",
                notification.Id, nameof(EquipmentReservedEvent));
            return;
        }

        equipment.Reserve(notification.Quantity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}