using Microsoft.EntityFrameworkCore;
using StudioManager.API.Contracts.Reservations;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Entities;
using StudioManager.Domain.ErrorMessages;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure.Common;

namespace StudioManager.Application.Reservations.Common;

public static class ReservationsChecker
{
    public static async Task<CommandResult> CheckReservationAsync(
        DbContextBase dbContext,
        ReservationWriteDto newReservation,
        Guid? notId = null,
        CancellationToken cancellationToken = default)
    {
        var equipment = await dbContext.GetEquipmentAsync(newReservation.EquipmentId, cancellationToken);

        if (equipment is null) return CommandResult.Conflict(DB.RESERVATION_EQUIPMENT_NOT_FOUND);

        if (equipment.Quantity - newReservation.Quantity < 0)
            return CommandResult.Conflict(DB.RESERVATION_EQUIPMENT_QUANTITY_INSUFFICIENT);

        var existingReservationsFilter = new ReservationFilter
        {
            NotId = notId,
            MinEndDate = newReservation.StartDate,
            MaxStartDate = newReservation.EndDate,
            EquipmentId = newReservation.EquipmentId
        };

        var reservations = await dbContext.GetReservationsAsync(existingReservationsFilter, cancellationToken);

        if (reservations.Count <= 0) return CommandResult.Success();

        var reservedQuantities = reservations.Sum(r => r.Quantity);
        return equipment.Quantity - reservedQuantities - newReservation.Quantity < 0
            ? CommandResult.Conflict(DB.RESERVATION_EQUIPMENT_USED_BY_OTHERS_IN_PERIOD)
            : CommandResult.Success();
    }
    
    public static async Task<CommandResult> CheckReservationUserAsync(
        DbContextBase dbContext,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var userExists = await dbContext.Users.AnyAsync(x => x.Id == userId, cancellationToken);

        return !userExists
            ? CommandResult.NotFound<User>(userId)
            : CommandResult.Success();
    }
}
