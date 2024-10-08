﻿using Microsoft.EntityFrameworkCore;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Application.Equipments.Common;
using StudioManager.Domain.Entities;
using StudioManager.Infrastructure;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.Equipments.Update;

public sealed class UpdateEquipmentCommandHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory)
    : ICommandHandler<UpdateEquipmentCommand>
{
    public async Task<CommandResult> Handle(UpdateEquipmentCommand request, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var checkResult = await EquipmentChecker.CheckEquipmentReferencesAsync(
            dbContext,
            request.Id,
            request.Equipment,
            cancellationToken);

        if (!checkResult.Succeeded) return checkResult.CommandResult;

        var equipment = await dbContext.GetEquipmentAsync(request.Id, cancellationToken);

        if (equipment is null) return CommandResult.NotFound<Equipment>(request.Id);

        equipment.Update(request.Equipment.Name, request.Equipment.EquipmentTypeId, request.Equipment.Quantity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return CommandResult.Success();
    }
}
