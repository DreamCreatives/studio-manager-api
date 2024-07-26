using StudioManager.API.Contracts.Equipments;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Domain.Entities;
using StudioManager.Domain.ErrorMessages;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure.Common;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.Equipments.Common;

public static class EquipmentChecker
{
    internal static async Task<CheckResult> CheckEquipmentReferencesAsync(
        DbContextBase dbContext,
        Guid? equipmentId,
        EquipmentWriteDto dto,
        CancellationToken cancellationToken = default)
    {
        var result = await CheckIfEquipmentTypeExistAsync(dbContext, dto.EquipmentTypeId, cancellationToken);

        if (!result.Succeeded) return CheckResult.Fail(result.CommandResult);

        var filter = new EquipmentFilter
        {
            NotId = equipmentId,
            ExactName = dto.Name,
            EquipmentTypeId = dto.EquipmentTypeId
        };

        return await EquipmentHasUniqueName(dbContext, filter, cancellationToken);
    }

    private static async Task<CheckResult> EquipmentHasUniqueName(DbContextBase dbContext,
        EquipmentFilter filter,
        CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.GetEquipmentAsync(filter, cancellationToken);

        return existing is null
            ? CheckResult.Success()
            : CheckResult.Fail(
                CommandResult.Conflict(
                    string.Format(DB_FORMAT.EQUIPMENT_DUPLICATE_NAME_TYPE,
                        existing.Name, existing.EquipmentTypeId)));
    }

    private static async Task<CheckResult> CheckIfEquipmentTypeExistAsync(
        DbContextBase dbContext,
        Guid equipmentTypeId,
        CancellationToken cancellationToken = default)
    {
        var filter = new EquipmentTypeFilter { Id = equipmentTypeId };
        var typeExistsAsync = await dbContext.EquipmentTypeExistsAsync(filter, cancellationToken);

        return typeExistsAsync
            ? CheckResult.Success()
            : CheckResult.Fail(CommandResult.NotFound<EquipmentType>(equipmentTypeId));
    }
}
