using MediatR;
using Microsoft.EntityFrameworkCore;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Entities;
using StudioManager.Domain.ErrorMessages;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure;

namespace StudioManager.Application.Equipments.Delete;

public sealed class DeleteEquipmentCommandHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory)
    : IRequestHandler<DeleteEquipmentCommand, CommandResult>
{
    public async Task<CommandResult> Handle(DeleteEquipmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var filter = GetFilter();
            var equipment = await context.GetEquipmentAsync(filter, cancellationToken);

            if (equipment is null)
            {
                return CommandResult.NotFound<Equipment>(request.Id);
            }
            
            if (!HasInitialQuantity(equipment))
            {
                return CommandResult.Conflict(
                    string.Format(DB_FORMAT.EQUIPMENT_QUANTITY_MISSING_WHEN_REMOVING,
                        equipment.InitialQuantity,
                        equipment.Quantity));
            }

            context.Equipments.Remove(equipment);
            await context.SaveChangesAsync(cancellationToken);
            return CommandResult.Success();
        }
        catch (Exception ex)
        {
            return CommandResult.UnexpectedError(ex.Message);
        }

        EquipmentFilter GetFilter() => new() { Id = request.Id };

        bool HasInitialQuantity(Equipment eq) => eq.InitialQuantity == eq.Quantity;
    }
}