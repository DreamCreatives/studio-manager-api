using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StudioManager.API.Contracts.Pagination;
using StudioManager.API.Contracts.Reservations;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Domain.Common.Results;
using StudioManager.Infrastructure;

namespace StudioManager.Application.Reservations.GetAll;

public sealed class GetAllReservationsQueryHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory,
    IMapper mapper)
    : IQueryHandler<GetAllReservationsQuery, PagingResultDto<ReservationReadDto>>
{
    public async Task<QueryResult<PagingResultDto<ReservationReadDto>>> Handle(GetAllReservationsQuery request,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var data = dbContext.Reservations
            .AsNoTracking()
            .Where(request.Filter.ToQuery())
            .ProjectTo<ReservationReadDto>(mapper.ConfigurationProvider);

        return await data.ApplyPagingAsync(request.Pagination);
    }
}
