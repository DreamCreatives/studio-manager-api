using Microsoft.EntityFrameworkCore;
using StudioManager.Domain.Entities;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure.Common;

namespace StudioManager.Application.DbContextExtensions;

public static class EquipmentTypeDatabaseExtensions
{
    public static async Task<bool> EquipmentTypeExistsAsync(
        this DbContextBase dbContext,
        EquipmentTypeFilter filter,
        CancellationToken cancellationToken = default)
    {
        return await dbContext
            .EquipmentTypes
            .AnyAsync(filter.ToQuery(), cancellationToken);
    }
    
    public static async Task<EquipmentType?> GetEquipmentTypeAsync(
        this DbContextBase dbContext,
        EquipmentTypeFilter filter,
        CancellationToken cancellationToken = default)
    {
        return await dbContext
            .EquipmentTypes
            .FirstOrDefaultAsync(filter.ToQuery(), cancellationToken);
    }
}