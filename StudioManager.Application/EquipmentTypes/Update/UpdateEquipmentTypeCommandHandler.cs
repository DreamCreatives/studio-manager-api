using Microsoft.EntityFrameworkCore;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Domain.Entities;
using StudioManager.Domain.ErrorMessages;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.EquipmentTypes.Update;

public sealed class UpdateEquipmentTypeCommandHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory)
    : ICommandHandler<UpdateEquipmentTypeCommand>
{
    public async Task<CommandResult> Handle(UpdateEquipmentTypeCommand request, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var filter = CreateUniqueFilter();
        var exists = await dbContext.EquipmentTypeExistsAsync(filter, cancellationToken);

        if (exists) return CommandResult.Conflict(DB.EQUIPMENT_TYPE_DUPLICATE_NAME);

        filter = CreateFilter();
        var dbEquipmentType = await dbContext.EquipmentTypes.FirstOrDefaultAsync(filter.ToQuery(), cancellationToken);
        if (dbEquipmentType is null) return CommandResult.NotFound<EquipmentType>(request.Id);

        dbEquipmentType.Update(request.EquipmentType.Name);

        await dbContext.SaveChangesAsync(cancellationToken);
        return CommandResult.Success();

        EquipmentTypeFilter CreateFilter()
        {
            return new EquipmentTypeFilter { Id = request.Id };
        }

        EquipmentTypeFilter CreateUniqueFilter()
        {
            return new EquipmentTypeFilter { NotId = request.Id, ExactName = request.EquipmentType.Name };
        }
    }
}
