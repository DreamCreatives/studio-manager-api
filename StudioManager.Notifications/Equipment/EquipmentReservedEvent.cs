using MediatR;

namespace StudioManager.Notifications.Equipment;

public sealed class EquipmentReservedEvent(Guid id, int quantity) : INotification
{
    public Guid Id { get; } = id;
    public int Quantity { get; } = quantity;
}