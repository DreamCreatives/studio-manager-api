using MediatR;
using StudioManager.API.Contracts.Reservations;
using StudioManager.Domain.Common.Results;

namespace StudioManager.Application.Reservations.Create;

public sealed record CreateReservationCommand(ReservationWriteDto Reservation ) : IRequest<CommandResult>;