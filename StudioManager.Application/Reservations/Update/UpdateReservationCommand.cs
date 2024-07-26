using StudioManager.API.Contracts.Reservations;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.Reservations.Update;

public sealed record UpdateReservationCommand(Guid Id, ReservationWriteDto Reservation) : ICommand;
