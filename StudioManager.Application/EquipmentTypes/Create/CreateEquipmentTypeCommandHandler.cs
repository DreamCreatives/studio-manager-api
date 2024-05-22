using MediatR;
using Microsoft.EntityFrameworkCore;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Entities;
using StudioManager.Domain.ErrorMessages;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure;

namespace StudioManager.Application.EquipmentTypes.Create;

public sealed class CreateEquipmentTypeCommandHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory)
    : IRequestHandler<CreateEquipmentTypeCommand, CommandResult>
{
    public async Task<CommandResult> Handle(CreateEquipmentTypeCommand request, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var filter = CreateFilter();
        var exists = await dbContext.EquipmentTypeExistsAsync(filter, cancellationToken);

        if (exists)
        {
            return CommandResult.Conflict(DB.EQUIPMENT_TYPE_DUPLICATE_NAME);
        }
        
        var equipmentType = EquipmentType.Create(request.EquipmentType.Name);
        await dbContext.EquipmentTypes.AddAsync(equipmentType, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return CommandResult.Success(equipmentType.Id);

        EquipmentTypeFilter CreateFilter() => new() { ExactName = request.EquipmentType.Name };
    }
}