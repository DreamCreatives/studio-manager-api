using StudioManager.API.Contracts.Common;
using StudioManager.API.Contracts.Users;

namespace StudioManager.API.Contracts.Reservations;

public sealed record ReservationReadDto(
    Guid Id,
    DateOnly StartDate,
    DateOnly EndDate,
    int Quantity,
    UserReadDto User,
    NamedBaseDto Equipment);
