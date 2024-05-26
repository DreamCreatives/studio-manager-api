using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StudioManager.API.Contracts.Reservations;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.ErrorMessages;
using StudioManager.Infrastructure;

namespace StudioManager.Application.Reservations.GetById;

public sealed class GetReservationByIdQueryHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory,
    IMapper mapper)
    : IQueryHandler<GetReservationByIdQuery, ReservationReadDto>
{
    public async Task<QueryResult<ReservationReadDto>> Handle(GetReservationByIdQuery request,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var reservation = await dbContext.Reservations
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .ProjectTo<ReservationReadDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return reservation is null
            ? QueryResult<ReservationReadDto>.NotFound(string.Format(DB_FORMAT.RESERVATION_NOT_FOUND, request.Id))
            : QueryResult.Success(reservation);
    }
}
