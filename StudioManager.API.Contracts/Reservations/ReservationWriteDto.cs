namespace StudioManager.API.Contracts.Reservations;

public sealed record ReservationWriteDto(DateOnly StartDate, DateOnly EndDate, int Quantity, Guid EquipmentId);