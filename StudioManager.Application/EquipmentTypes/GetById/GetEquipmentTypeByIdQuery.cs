using StudioManager.API.Contracts.EquipmentTypes;
using StudioManager.Domain.Common.Results;

namespace StudioManager.Application.EquipmentTypes.GetById;

public sealed record GetEquipmentTypeByIdQuery(Guid Id) : IQuery<EquipmentTypeReadDto>
{
    public Guid Id { get; } = Id;
}
