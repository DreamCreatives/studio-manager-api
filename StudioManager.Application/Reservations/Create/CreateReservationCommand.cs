using StudioManager.API.Contracts.Reservations;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.Reservations.Create;

public sealed record CreateReservationCommand(ReservationWriteDto Reservation) : ICommand;
