using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StudioManager.API.Contracts.EquipmentTypes;
using StudioManager.Infrastructure;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.EquipmentTypes.GetAll;

public sealed class GetEquipmentTypesQueryHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory,
    IMapper mapper)
    : IQueryHandler<GetEquipmentTypesQuery, List<EquipmentTypeReadDto>>
{
    public async Task<QueryResult<List<EquipmentTypeReadDto>>> Handle(
        GetEquipmentTypesQuery request,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var data = await dbContext.EquipmentTypes
            .AsNoTracking()
            .Where(request.Filter.ToQuery())
            .ProjectTo<EquipmentTypeReadDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return QueryResult.Success(data);
    }
}
