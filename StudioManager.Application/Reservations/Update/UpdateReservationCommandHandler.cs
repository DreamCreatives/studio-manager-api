using MediatR;
using Microsoft.EntityFrameworkCore;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Application.Reservations.Common;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Entities;
using StudioManager.Infrastructure;

namespace StudioManager.Application.Reservations.Update;

public sealed class UpdateReservationCommandHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory)
    : IRequestHandler<UpdateReservationCommand, CommandResult>
{
    public async Task<CommandResult> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = request.Reservation;
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        
        var checkResult = await ReservationsChecker.CheckReservationAsync(dbContext, reservation, request.Id, cancellationToken);
        
        if (!checkResult.Succeeded)
        {
            return checkResult;
        }
        
        var dbReservation = await dbContext.GetReservationAsync(request.Id, cancellationToken);

        if (dbReservation is null)
        {
            return CommandResult.NotFound<Reservation>(request.Id);
        }
        
        dbReservation.Update(reservation.StartDate, reservation.EndDate, reservation.Quantity, reservation.EquipmentId);

        dbContext.Reservations.Update(dbReservation);
        await dbContext.SaveChangesAsync(cancellationToken);

        return CommandResult.Success();
    }
}