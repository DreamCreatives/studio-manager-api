using StudioManager.API.Contracts.EquipmentTypes;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.EquipmentTypes.GetAll;

public sealed class GetEquipmentTypesQuery : IQuery<List<EquipmentTypeReadDto>>
{
    public required EquipmentTypeFilter Filter { get; init; }
}
