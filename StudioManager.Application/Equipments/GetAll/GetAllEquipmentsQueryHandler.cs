using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StudioManager.API.Contracts.Equipments;
using StudioManager.API.Contracts.Pagination;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Domain.Common.Results;
using StudioManager.Infrastructure;

namespace StudioManager.Application.Equipments.GetAll;

public sealed class GetAllEquipmentsQueryHandler(
    IMapper mapper,
    IDbContextFactory<StudioManagerDbContext> dbContextFactory)
    : IQueryHandler<GetAllEquipmentsQuery, PagingResultDto<EquipmentReadDto>>
{
    public async Task<QueryResult<PagingResultDto<EquipmentReadDto>>> Handle(GetAllEquipmentsQuery request,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var data = dbContext.Equipments
            .AsNoTracking()
            .Where(request.Filter.ToQuery())
            .ProjectTo<EquipmentReadDto>(mapper.ConfigurationProvider);

        return await data.ApplyPagingAsync(request.Pagination);
    }
}
