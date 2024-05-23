﻿using MediatR;
using StudioManager.Domain.Common.Results;

namespace StudioManager.Application.Reservations.Delete;

public sealed record DeleteReservationCommand(Guid Id) : IRequest<CommandResult>;
