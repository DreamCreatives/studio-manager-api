using StudioManager.Notifications.Equipment;

namespace StudioManager.Domain.Entities;

public sealed class Reservation : EntityBase
{
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public int Quantity { get; private set; }
    public Guid EquipmentId { get; private set; }
    public Guid UserId { get; private set; }

    
    #region EntityRelations

    public Equipment Equipment { get; init; } = null!;
    public User User { get; init; } = null!;
    
    
    #endregion

    public static Reservation Create(
        DateOnly startDate,
        DateOnly endDate,
        int quantity,
        Guid equipmentId,
        Guid userId)
    {
        return new Reservation
        {
            Id = Guid.NewGuid(),
            StartDate = startDate,
            EndDate = endDate,
            Quantity = quantity,
            EquipmentId = equipmentId,
            UserId = userId
        };
    }

    public void Update(
        DateOnly startDate,
        DateOnly endDate,
        int quantity,
        Guid equipmentId)
    {
        AddDomainEvent(new EquipmentReservationChangedEvent(equipmentId, quantity, Quantity));

        StartDate = startDate;
        EndDate = endDate;
        Quantity = quantity;
        EquipmentId = equipmentId;
    }
}
