using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure;
using StudioManager.Infrastructure.Common;
using StudioManager.Notifications.Equipment;

namespace StudioManager.API.BackgroundServices;

[ExcludeFromCodeCoverage]
//TODO: Create read lock for this service
public sealed class FinishedReservationsBackgroundService(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);

            await using var dbContext = await dbContextFactory.CreateDbContextAsync(stoppingToken);
            {
                await ReturnReservationsAsync(dbContext, stoppingToken);
            }
        }
    }

    private static async Task ReturnReservationsAsync(DbContextBase dbContext, CancellationToken cancellationToken)
    {
        var filter = new ReservationFilter { EndDate = DateOnly.FromDateTime(DateTime.Today) };

        var reservations = await dbContext.GetReservationsAsync(filter, cancellationToken);

        if (reservations.Count == 0)
        {
            return;
        }
            
        foreach (var reservation in reservations)
        {
            reservation.AddDomainEvent(new EquipmentReturnedEvent(reservation.EquipmentId, 0));
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}