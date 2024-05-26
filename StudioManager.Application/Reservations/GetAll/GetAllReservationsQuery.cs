using StudioManager.API.Contracts.Pagination;
using StudioManager.API.Contracts.Reservations;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Filters;

namespace StudioManager.Application.Reservations.GetAll;

public sealed class GetAllReservationsQuery(
    ReservationFilter filter,
    PaginationDto pagination)
    : IQuery<PagingResultDto<ReservationReadDto>>
{
    public ReservationFilter Filter { get; } = filter;
    public PaginationDto Pagination { get; } = pagination;
}
