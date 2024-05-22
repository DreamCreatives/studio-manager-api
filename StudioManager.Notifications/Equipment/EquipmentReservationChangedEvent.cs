using MediatR;

namespace StudioManager.Notifications.Equipment;

public sealed class EquipmentReservationChangedEvent(Guid equipmentId, int quantity, int initialQuantity) : INotification
{
    public Guid EquipmentId { get; } = equipmentId;
    public int Quantity { get; } = quantity;
    public int InitialQuantity { get; } = initialQuantity;
}