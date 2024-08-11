using Microsoft.EntityFrameworkCore;
using StudioManager.Application.Common;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.Reservations.Update;

public sealed class UpdateReservationCommandAuthorizationHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory,
    ITokenDecryptor tokenDecryptor)
    : IAuthorizationHandler<UpdateReservationCommand>
{
    public async Task<CommandResult> AuthorizeAsync(UpdateReservationCommand request, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var stringUserId = tokenDecryptor.UserId;
        
        if (!Guid.TryParse(stringUserId, out var userId))
        {
            return CommandResult.Forbidden();
        }
        
        var filter = new ReservationFilter { Id = request.Id, UserId = userId };

        var reservation = await dbContext.GetReservationAsync(filter, cancellationToken);

        return reservation is not null
            ? CommandResult.Success()
            : CommandResult.Forbidden();
    }
}
