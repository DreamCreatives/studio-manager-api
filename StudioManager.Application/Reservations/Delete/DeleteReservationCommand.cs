using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.Reservations.Delete;

public sealed record DeleteReservationCommand(Guid Id) : ICommand;
