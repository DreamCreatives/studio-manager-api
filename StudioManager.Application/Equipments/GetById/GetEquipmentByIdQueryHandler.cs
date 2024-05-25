using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudioManager.API.Contracts.Equipments;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.ErrorMessages;
using StudioManager.Infrastructure;

namespace StudioManager.Application.Equipments.GetById;

public sealed class GetEquipmentByIdQueryHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory,
    IMapper mapper)
    : IRequestHandler<GetEquipmentByIdQuery, QueryResult<EquipmentReadDto>>
{
    public async Task<QueryResult<EquipmentReadDto>> Handle(GetEquipmentByIdQuery request, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var equipment = await dbContext.Equipments
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .ProjectTo<EquipmentReadDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return equipment is null
            ? QueryResult<EquipmentReadDto>.NotFound(string.Format(DB_FORMAT.EQUIPMENT_DOES_NOT_EXIST, request.Id))
            : QueryResult.Success(equipment);
    }
}
