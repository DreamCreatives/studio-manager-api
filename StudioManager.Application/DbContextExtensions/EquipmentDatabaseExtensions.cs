using Microsoft.EntityFrameworkCore;
using StudioManager.Domain.Entities;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure.Common;

namespace StudioManager.Application.DbContextExtensions;

public static class EquipmentDatabaseExtensions
{
    public static async Task<Equipment?> GetEquipmentAsync(
        this DbContextBase dbContext,
        EquipmentFilter filter,
        CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Equipments
            .Where(filter.ToQuery())
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public static async Task<Equipment?> GetEquipmentAsync(
        this DbContextBase dbContext, 
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var filter = new EquipmentFilter { Id = id };
        return await dbContext.GetEquipmentAsync(filter, cancellationToken);
    }
}