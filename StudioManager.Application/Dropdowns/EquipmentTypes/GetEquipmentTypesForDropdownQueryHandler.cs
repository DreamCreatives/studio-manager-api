using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StudioManager.API.Contracts.Common;
using StudioManager.Domain.Common.EnumerableExtensions;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure;

namespace StudioManager.Application.Dropdowns.EquipmentTypes;

public sealed class GetEquipmentTypesForDropdownQueryHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory,
    IMapper mapper)
    : IQueryHandler<GetEquipmentTypesForDropdownQuery, IReadOnlyList<NamedBaseDto>>
{
    public async Task<QueryResult<IReadOnlyList<NamedBaseDto>>> Handle(GetEquipmentTypesForDropdownQuery request, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        
        var filter = new EquipmentTypeFilter { Search = request.Search };

        var equipments = await dbContext.EquipmentTypes
            .Where(filter.ToQuery())
            .OrderBy(x => x.Name)
            .ProjectTo<NamedBaseDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return QueryResult.Success(equipments.MakeReadOnly());
    }
}
