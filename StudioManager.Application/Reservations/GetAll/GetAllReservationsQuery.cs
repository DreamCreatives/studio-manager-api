using MediatR;
using StudioManager.API.Contracts.Pagination;
using StudioManager.API.Contracts.Reservations;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Filters;

namespace StudioManager.Application.Reservations.GetAll;

public sealed class GetAllReservationsQuery(
    ReservationFilter filter,
    PaginationDto pagination)
    : IRequest<QueryResult<PagingResultDto<ReservationReadDto>>>
{
    public ReservationFilter Filter { get; } = filter;
    public PaginationDto Pagination { get; } = pagination;
}