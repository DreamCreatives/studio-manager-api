using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudioManager.API.Contracts.EquipmentTypes;
using StudioManager.Infrastructure;

namespace StudioManager.Application.EquipmentTypes.GetAll;

public sealed class GetEquipmentTypesQueryHandler(
    IDbContextFactory<StudioManagerReadDbContext> dbContextFactory,
    IMapper mapper)
    : IRequestHandler<GetEquipmentTypesQuery, IReadOnlyList<EquipmentTypeReadDto>>
{
    public async Task<IReadOnlyList<EquipmentTypeReadDto>> Handle(
        GetEquipmentTypesQuery request,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.EquipmentTypes
            .ProjectTo<EquipmentTypeReadDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}