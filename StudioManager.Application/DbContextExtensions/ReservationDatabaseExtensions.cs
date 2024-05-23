using Microsoft.EntityFrameworkCore;
using StudioManager.Domain.Entities;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure.Common;

namespace StudioManager.Application.DbContextExtensions;

public static class ReservationDatabaseExtensions
{
    public static async Task<IReadOnlyList<Reservation>> GetReservationsAsync(
        this DbContextBase dbContext,
        ReservationFilter filter,
        CancellationToken cancellationToken)
    {
        return await dbContext.Reservations
            .Where(filter.ToQuery())
            .ToListAsync(cancellationToken);
    }

    public static async Task<Reservation?> GetReservationAsync(
        this DbContextBase dbContext,
        Guid id,
        CancellationToken cancellationToken)
    {
        var filter = new ReservationFilter { Id = id };
        return await dbContext.Reservations
            .Where(filter.ToQuery())
            .FirstOrDefaultAsync(cancellationToken);
    }
}
