using Microsoft.EntityFrameworkCore;
using StudioManager.Domain.Entities;
using StudioManager.Infrastructure;

namespace StudioManager.Application.Tests.EquipmentTypes.Common;

internal static class EquipmentTypeTestContextHelper
{
    internal static async Task AddEquipmentTypeAsync(StudioManagerDbContext context, params EquipmentType[] equipmentType)
    {
        await context.EquipmentTypes.AddRangeAsync(equipmentType);
        await context.SaveChangesAsync();
    }

    internal static async Task ClearEquipmentTypesAsync(StudioManagerDbContext context)
    {
        await context.EquipmentTypes.ExecuteDeleteAsync();
        await context.SaveChangesAsync();
    }
}