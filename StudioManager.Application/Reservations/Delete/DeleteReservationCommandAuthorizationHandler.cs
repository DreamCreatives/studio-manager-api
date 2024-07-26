using Microsoft.EntityFrameworkCore;
using StudioManager.Application.Common;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.Reservations.Delete;

public sealed class DeleteReservationCommandAuthorizationHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory,
    ITokenDecryptor tokenDecryptor)
    : IAuthorizationHandler<DeleteReservationCommand>
{
    public async Task<CommandResult> AuthorizeAsync(DeleteReservationCommand command, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var stringUserId = tokenDecryptor.UserId;
        
        if (!Guid.TryParse(stringUserId, out var userId))
        {
            return CommandResult.Forbidden();
        }
        
        var filter = new ReservationFilter { Id = command.Id, UserId = userId };

        var reservation = await dbContext.GetReservationAsync(filter, cancellationToken);

        return reservation is not null
            ? CommandResult.Success()
            : CommandResult.Forbidden();
    }
}
