using StudioManager.API.Contracts.Pagination;
using StudioManager.API.Contracts.Reservations;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.Reservations.GetAll;

public sealed class GetAllReservationsQuery(
    ReservationFilter filter,
    PaginationDto pagination)
    : IQuery<PagingResultDto<ReservationReadDto>>
{
    public ReservationFilter Filter { get; } = filter;
    public PaginationDto Pagination { get; } = pagination;
}
