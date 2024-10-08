﻿using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.Users.Delete;

public sealed record DeleteUserCommand(Guid Id) : ICommand;
