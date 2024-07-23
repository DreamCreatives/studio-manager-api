using Microsoft.EntityFrameworkCore;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Application.KeyCloak;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Entities;
using StudioManager.Infrastructure;

namespace StudioManager.Application.Users.Delete;

public sealed class DeleteUserCommandHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory,
    IKeyCloakService keyCloakService)
    : ICommandHandler<DeleteUserCommand>
{
    public async Task<CommandResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var existingUser = await dbContext.GetUserByIdAsync(request.Id, cancellationToken);

        if (existingUser is null)
        {
            return CommandResult.NotFound<User>(request.Id);
        }

        var keyCloakResult = await keyCloakService.RemoveUserAsync(existingUser.KeycloakId, cancellationToken);

        if (!keyCloakResult.Succeeded)
        {
            return keyCloakResult;
        }

        dbContext.Users.Remove(existingUser);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return CommandResult.Success();
    }
}
