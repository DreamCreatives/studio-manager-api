using MediatR;
using Microsoft.EntityFrameworkCore;
using StudioManager.Application.Reservations.Common;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Entities;
using StudioManager.Infrastructure;
using StudioManager.Notifications.Equipment;

namespace StudioManager.Application.Reservations.Create;

public sealed class CreateReservationCommandHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory)
    : IRequestHandler<CreateReservationCommand, CommandResult>
{
    public async Task<CommandResult> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = request.Reservation;
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        
        var checkResult = await ReservationsChecker.CheckReservationAsync(dbContext, reservation, null, cancellationToken);
        
        if (!checkResult.Succeeded)
        {
            return checkResult;
        }
        
        var dbReservation = Reservation.Create(
            reservation.StartDate,
            reservation.EndDate,
            reservation.Quantity,
            reservation.EquipmentId);

        await dbContext.Reservations.AddAsync(dbReservation, cancellationToken);
        
        dbReservation.AddDomainEvent(new EquipmentReservedEvent(reservation.EquipmentId, reservation.Quantity));
        await dbContext.SaveChangesAsync(cancellationToken);

        return CommandResult.Success(dbReservation.Id);
    }
}