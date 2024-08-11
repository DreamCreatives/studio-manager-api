using StudioManager.API.Contracts.Reservations;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.Reservations.GetById;

public sealed record GetReservationByIdQuery(Guid Id) : IQuery<ReservationReadDto>
{
    public Guid Id { get; } = Id;
}
