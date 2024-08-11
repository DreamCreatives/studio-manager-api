using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StudioManager.API.Contracts.EquipmentTypes;
using StudioManager.Domain.ErrorMessages;
using StudioManager.Infrastructure;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.EquipmentTypes.GetById;

public sealed class GetEquipmentTypeByIdQueryHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory,
    IMapper mapper)
    : IQueryHandler<GetEquipmentTypeByIdQuery, EquipmentTypeReadDto>
{
    public async Task<QueryResult<EquipmentTypeReadDto>> Handle(GetEquipmentTypeByIdQuery request,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var equipmentType = await dbContext.EquipmentTypes
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .ProjectTo<EquipmentTypeReadDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return equipmentType is null
            ? QueryResult<EquipmentTypeReadDto>.NotFound(string.Format(DB_FORMAT.EQUIPMENT_TYPE_NOT_FOUND, request.Id))
            : QueryResult.Success(equipmentType);
    }
}
