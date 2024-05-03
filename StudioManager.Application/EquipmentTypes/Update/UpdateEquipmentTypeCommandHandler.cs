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
            var filter = CreateFilter();
            var dbEquipmentType = await dbContext.EquipmentTypes.FirstOrDefaultAsync(filter.ToQuery(), cancellationToken);
            if (dbEquipmentType is null)
            {
                return CommandResult.NotFound<EquipmentType>();
            }

            filter = CreateUniqueFilter();
            
            var exists = await dbContext.EquipmentTypeExistsAsync(filter, cancellationToken);
            
            if (exists)
            {
                return CommandResult.Conflict(DB.EQUIPMENT_TYPE_NON_UNIQUE_NAME);
            }
            
            dbEquipmentType.Update(request.EquipmentType.Name);

            return CommandResult.Success();
        }
        catch (DbUpdateException e)
        {
            return CommandResult.UnexpectedError(e.Message);
        }
        
        EquipmentTypeFilter CreateUniqueFilter() => new() { Id = request.Id, Name = request.EquipmentType.Name };
        EquipmentTypeFilter CreateFilter() => new() { Id = request.Id };
    }
}