using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StudioManager.API.Contracts.Common;
using StudioManager.Domain.Common.EnumerableExtensions;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure;

namespace StudioManager.Application.Dropdowns.Equipments;

public sealed class GetEquipmentsForDropdownQueryHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory,
    IMapper mapper)
    : IQueryHandler<GetEquipmentsForDropdownQuery, IReadOnlyList<NamedBaseDto>>
{
    public async Task<QueryResult<IReadOnlyList<NamedBaseDto>>> Handle(GetEquipmentsForDropdownQuery request, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        
        var filter = new EquipmentFilter { Search = request.Search };

        var equipments = (await dbContext.Equipments
            .Where(filter.ToQuery())
            .OrderBy(x => x.Name)
            .ProjectTo<NamedBaseDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken)).MakeReadOnly();

        return QueryResult.Success(equipments);
    }
}
