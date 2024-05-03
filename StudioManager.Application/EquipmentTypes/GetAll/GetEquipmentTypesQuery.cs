using MediatR;
using StudioManager.API.Contracts.EquipmentTypes;
using StudioManager.Domain.Filters;

namespace StudioManager.Application.EquipmentTypes.GetAll;

public sealed class GetEquipmentTypesQuery : IRequest<IReadOnlyList<EquipmentTypeReadDto>>
{
    public EquipmentTypeFilter Filter { get; init; } = null!;
}