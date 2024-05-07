using MediatR;
using StudioManager.API.Contracts.Equipment;
using StudioManager.API.Contracts.Pagination;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Filters;

namespace StudioManager.Application.Equipments.GetAll;

public sealed class GetAllEquipmentsQuery(
    EquipmentFilter filter,
    PaginationDto pagination)
    : IRequest<QueryResult<PagingResultDto<EquipmentReadDto>>>
{
    public EquipmentFilter Filter { get; } = filter;
    public PaginationDto Pagination { get; } = pagination;
}