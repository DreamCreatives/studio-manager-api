using MediatR;
using StudioManager.API.Contracts.Reservations;
using StudioManager.Domain.Common.Results;

namespace StudioManager.Application.Reservations.BetById;

public sealed record GetReservationByIdQuery(Guid Id) : IRequest<QueryResult<ReservationReadDto>>
{
    public Guid Id { get; } = Id;
}
