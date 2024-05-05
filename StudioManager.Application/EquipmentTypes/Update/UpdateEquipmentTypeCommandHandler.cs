using MediatR;
using Microsoft.EntityFrameworkCore;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Entities;
using StudioManager.Domain.ErrorMessages;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure;

namespace StudioManager.Application.EquipmentTypes.Update;

public sealed class UpdateEquipmentTypeCommandHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory)
    : IRequestHandler<UpdateEquipmentTypeCommand, CommandResult>
{
    public async Task<CommandResult> Handle(UpdateEquipmentTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var filter = CreateUniqueFilter();
            var exists = await dbContext.EquipmentTypeExistsAsync(filter, cancellationToken);
            
            if (exists)
            {
                return CommandResult.Conflict(DB.EQUIPMENT_TYPE_DUPLICATE_NAME);
            }
            
            filter = CreateFilter();
            var dbEquipmentType = await dbContext.EquipmentTypes.FirstOrDefaultAsync(filter.ToQuery(), cancellationToken);
            if (dbEquipmentType is null)
            {
                return CommandResult.NotFound<EquipmentType>(request.Id);
            }
            
            dbEquipmentType.Update(request.EquipmentType.Name);

            await dbContext.SaveChangesAsync(cancellationToken);
            return CommandResult.Success();
        }
        catch (DbUpdateException e)
        {
            return CommandResult.UnexpectedError(e);
        }
        
        EquipmentTypeFilter CreateFilter() => new() { Id = request.Id };
        EquipmentTypeFilter CreateUniqueFilter() => new() { Id = request.Id, ExactName = request.EquipmentType.Name};
    }
}