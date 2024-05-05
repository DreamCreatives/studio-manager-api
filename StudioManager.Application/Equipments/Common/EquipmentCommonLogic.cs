using StudioManager.API.Contracts.Equipment;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Entities;
using StudioManager.Domain.ErrorMessages;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure.Common;

namespace StudioManager.Application.Equipments.Common;

public static class EquipmentCommonLogic
{

    internal static async Task<CheckResult<Guid>> CheckEntityReferencesAsync(
        DbContextBase dbContext,
        EquipmentWriteDto dto,
        CancellationToken cancellationToken = default)
    {
        return await CheckEntityReferencesAsync(dbContext, dto, null, cancellationToken);
    }
    
    
    private static async Task<CheckResult<Guid>> CheckEntityReferencesAsync(
        DbContextBase dbContext,
        EquipmentWriteDto dto,
        Guid? existingId = null,
        CancellationToken cancellationToken = default)
    {
        var filter = GetEquipmentTypeFilter();
        var equipmentType = await dbContext.GetEquipmentTypeAsync(filter, cancellationToken);

        if (equipmentType is null)
        {
            return CheckResult<Guid>.NotFound<EquipmentType>(dto.EquipmentTypeId);
        }

        var uniqueFilter = GetEquipmentFilter();
        var entityExists = await dbContext.EquipmentExistsAsync(uniqueFilter, cancellationToken);

        if (entityExists)
        {
            return CheckResult<Guid>.Conflict(
                string.Format(DB_FORMAT.EQUIPMENT_DUPLICATE_NAME_TYPE, dto.Name, equipmentType.Name));
        }

        return CheckResult<Guid>.Success(equipmentType.Id);

        EquipmentTypeFilter GetEquipmentTypeFilter() => new() { Id = dto.EquipmentTypeId };
        EquipmentFilter GetEquipmentFilter() => new()
        {
            NotId = existingId,
            ExactName = dto.Name,
            EquipmentTypeId = equipmentType.Id
        };
    }
}