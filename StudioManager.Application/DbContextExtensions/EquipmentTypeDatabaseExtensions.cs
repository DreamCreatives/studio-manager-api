using Microsoft.EntityFrameworkCore;
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
}