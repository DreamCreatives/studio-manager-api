using MediatR;
using StudioManager.API.Contracts.Reservations;
using StudioManager.Domain.Common.Results;

namespace StudioManager.Application.Reservations.Update;

public sealed record UpdateReservationCommand(Guid Id, ReservationWriteDto Reservation) : IRequest<CommandResult>;
