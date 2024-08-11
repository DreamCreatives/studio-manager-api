using Microsoft.EntityFrameworkCore;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Domain.Entities;
using StudioManager.Infrastructure;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.Reservations.Delete;

public sealed class DeleteReservationCommandHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory)
    : ICommandHandler<DeleteReservationCommand>
{
    public async Task<CommandResult> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var dbReservation = await dbContext.GetReservationAsync(request.Id, cancellationToken);

        if (dbReservation is null) return CommandResult.NotFound<Reservation>(request.Id);
        
        dbContext.Reservations.Remove(dbReservation);
        await dbContext.SaveChangesAsync(cancellationToken);
        return CommandResult.Success();
    }
}
