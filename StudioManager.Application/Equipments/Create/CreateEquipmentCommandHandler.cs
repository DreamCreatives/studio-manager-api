using Microsoft.EntityFrameworkCore;
using StudioManager.Application.Equipments.Common;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Entities;
using StudioManager.Infrastructure;

namespace StudioManager.Application.Equipments.Create;

public sealed class CreateEquipmentCommandHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory)
    : ICommandHandler<CreateEquipmentCommand>
{
    public async Task<CommandResult> Handle(CreateEquipmentCommand request, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var checkResult =
            await EquipmentChecker.CheckEquipmentReferencesAsync(dbContext, null, request.Equipment, cancellationToken);

        if (!checkResult.Succeeded) return checkResult.CommandResult;

        var equipment = Equipment.Create(
            request.Equipment.Name,
            request.Equipment.EquipmentTypeId,
            request.Equipment.Quantity);

        await dbContext.Equipments.AddAsync(equipment, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return CommandResult.Success(equipment.Id);
    }
}
