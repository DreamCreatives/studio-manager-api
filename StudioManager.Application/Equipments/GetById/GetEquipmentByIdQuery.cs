using MediatR;
using StudioManager.API.Contracts.Equipments;
using StudioManager.Domain.Common.Results;

namespace StudioManager.Application.Equipments.GetById;

public sealed record GetEquipmentByIdQuery(Guid Id) : IRequest<QueryResult<EquipmentReadDto>>
{
    public Guid Id { get; } = Id;
}
