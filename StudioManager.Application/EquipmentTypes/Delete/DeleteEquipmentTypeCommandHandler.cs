using MediatR;
using Microsoft.EntityFrameworkCore;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Entities;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure;

namespace StudioManager.Application.EquipmentTypes.Delete;

public sealed class DeleteEquipmentTypeCommandHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory)
    : IRequestHandler<DeleteEquipmentTypeCommand, CommandResult>
{
    public async Task<CommandResult> Handle(DeleteEquipmentTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var filter = new EquipmentTypeFilter { Id = request.Id };
            var existing = await dbContext.GetEquipmentTypeAsync(filter, cancellationToken);
            
            if (existing is null)
            {
                return CommandResult.NotFound<EquipmentType>(request.Id);
            }

            dbContext.EquipmentTypes.Remove(existing);
            await dbContext.SaveChangesAsync(cancellationToken);
            return CommandResult.Success();
        }
        catch (DbUpdateException e)
        {
            return CommandResult.UnexpectedError(e.Message);
        }
    }
}