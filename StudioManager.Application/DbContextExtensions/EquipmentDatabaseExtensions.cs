using Microsoft.EntityFrameworkCore;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure.Common;

namespace StudioManager.Application.DbContextExtensions;

public static class EquipmentDatabaseExtensions
{
    public static async Task<bool> EquipmentExistsAsync(
        this DbContextBase dbContext,
        EquipmentFilter filter,
        CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Equipments
            .AnyAsync(filter.ToQuery(), cancellationToken);
    }
}