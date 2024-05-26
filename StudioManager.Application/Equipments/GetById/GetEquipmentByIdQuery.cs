using StudioManager.API.Contracts.Equipments;
using StudioManager.Domain.Common.Results;

namespace StudioManager.Application.Equipments.GetById;

public sealed record GetEquipmentByIdQuery(Guid Id) : IQuery<EquipmentReadDto>
{
    public Guid Id { get; } = Id;
}
