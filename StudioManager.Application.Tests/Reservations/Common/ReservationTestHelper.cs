﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using StudioManager.Domain.Entities;
using StudioManager.Infrastructure.Common;

namespace StudioManager.Application.Tests.Reservations.Common;

[ExcludeFromCodeCoverage]
internal static class ReservationTestHelper
{
    internal static async Task<Equipment> AddEquipmentAsync(DbContextBase dbContext)
    {
        await dbContext.EquipmentTypes.ExecuteDeleteAsync();
        await dbContext.Equipments.ExecuteDeleteAsync();
        var equipmentType = EquipmentType.Create("Test Equipment Type");
        var equipment = Equipment.Create("Test Equipment", equipmentType.Id, 100);
        
        await dbContext.EquipmentTypes.AddAsync(equipmentType);
        await dbContext.Equipments.AddAsync(equipment);
        await dbContext.SaveChangesAsync();
        return equipment;
    }

    internal static async Task<Reservation> AddReservationForDetailsAsync(
        DbContextBase dbContext,
        DateOnly startDate,
        DateOnly endDate,
        int? quantity = null)
    {
        var equipment = await AddEquipmentAsync(dbContext);
        
        var reservation = Reservation.Create(
            startDate,
            endDate,
            quantity ?? equipment.InitialQuantity,
            equipment.Id);

        await dbContext.Reservations.AddAsync(reservation);
        await dbContext.SaveChangesAsync();
        return reservation;
    }
    
    internal static async Task AddReservationForEquipmentAsync(DbContextBase dbContext,
        Guid equipmentId)
    {
        var reservation = Reservation.Create(
            DateOnly.FromDateTime(DateTime.UtcNow),
            DateOnly.FromDateTime(DateTime.UtcNow),
            1,
            equipmentId);

        await dbContext.Reservations.AddAsync(reservation);
        await dbContext.SaveChangesAsync();
    }
    
    internal static async Task<Reservation> AddReservationAsync(DbContextBase dbContext)
    {
        var equipment = await AddEquipmentAsync(dbContext);
        var reservation = Reservation.Create(
            DateOnly.FromDateTime(DateTime.UtcNow),
            DateOnly.FromDateTime(DateTime.UtcNow),
            1,
            equipment.Id);

        await dbContext.Reservations.AddAsync(reservation);
        await dbContext.SaveChangesAsync();
        return reservation;
    }
}