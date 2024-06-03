using Microsoft.EntityFrameworkCore;
using StudioManager.Application.Common;
using StudioManager.Application.Reservations.Common;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Entities;
using StudioManager.Domain.ErrorMessages;
using StudioManager.Infrastructure;
using StudioManager.Notifications.Equipment;

namespace StudioManager.Application.Reservations.Create;

public sealed class CreateReservationCommandHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory,
    ITokenDecryptor tokenDecryptor)
    : ICommandHandler<CreateReservationCommand>
{
    public async Task<CommandResult> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = request.Reservation;
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var checkResult =
            await ReservationsChecker.CheckReservationAsync(dbContext, reservation, null, cancellationToken);

        if (!checkResult.Succeeded) return checkResult;

        var userIdString = tokenDecryptor.UserId;
        
        if (!Guid.TryParse(userIdString, out var userId))
        {
            return CommandResult.Conflict(string.Format(DB_FORMAT.RESERVATION_INVALID_APP_ID, userIdString ?? "null"));
        }
        
        checkResult = await ReservationsChecker.CheckReservationUserAsync(dbContext, userId, cancellationToken);
        
        if (!checkResult.Succeeded) return checkResult;

        var dbReservation = Reservation.Create(
            reservation.StartDate,
            reservation.EndDate,
            reservation.Quantity,
            reservation.EquipmentId,
            userId);

        await dbContext.Reservations.AddAsync(dbReservation, cancellationToken);

        dbReservation.AddDomainEvent(new EquipmentReservedEvent(reservation.EquipmentId, reservation.Quantity));
        await dbContext.SaveChangesAsync(cancellationToken);

        return CommandResult.Success(dbReservation.Id);
    }
}
