using MediatR;

namespace StudioManager.Notifications.Equipment;

public sealed record EquipmentReturnedEvent(Guid Id, int Quantity) : INotification;