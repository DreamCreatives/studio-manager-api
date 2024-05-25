using MediatR;
using StudioManager.API.Contracts.EquipmentTypes;
using StudioManager.Domain.Common.Results;

namespace StudioManager.Application.EquipmentTypes.GetById;

public sealed record GetEquipmentTypeByIdQuery(Guid Id) : IRequest<QueryResult<EquipmentTypeReadDto>>
{
    public Guid Id { get; } = Id;
}
