using StudioManager.API.Contracts.Common;

namespace StudioManager.API.Contracts.Reservations;

public sealed record ReservationReadDto(
    Guid Id,
    DateOnly StartDate,
    DateOnly EndDate,
    int Quantity,
    NamedBaseDto Equipment);