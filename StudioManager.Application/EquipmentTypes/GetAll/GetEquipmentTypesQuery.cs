﻿using StudioManager.API.Contracts.EquipmentTypes;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Filters;

namespace StudioManager.Application.EquipmentTypes.GetAll;

public sealed class GetEquipmentTypesQuery : IQuery<List<EquipmentTypeReadDto>>
{
    public required EquipmentTypeFilter Filter { get; init; }
}
