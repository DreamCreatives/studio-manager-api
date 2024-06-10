using Microsoft.EntityFrameworkCore;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Entities;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure;

namespace StudioManager.Application.Equipments.Delete;

public sealed class DeleteEquipmentCommandHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory)
    : ICommandHandler<DeleteEquipmentCommand>
{
    public async Task<CommandResult> Handle(DeleteEquipmentCommand request, CancellationToken cancellationToken)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var filter = new EquipmentFilter { Id = request.Id };
        var equipment = await context.GetEquipmentAsync(filter, cancellationToken);

        if (equipment is null) return CommandResult.NotFound<Equipment>(request.Id);

        context.Equipments.Remove(equipment);
        await context.SaveChangesAsync(cancellationToken);
        return CommandResult.Success();
    }
}
