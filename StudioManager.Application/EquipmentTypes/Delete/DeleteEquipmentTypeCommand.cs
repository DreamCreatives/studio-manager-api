﻿using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.EquipmentTypes.Delete;

public sealed record DeleteEquipmentTypeCommand(Guid Id) : ICommand;
