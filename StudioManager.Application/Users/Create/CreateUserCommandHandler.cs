using FS.Keycloak.RestApiClient.Model;
using Microsoft.EntityFrameworkCore;
using StudioManager.Application.KeyCloak;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Entities;
using StudioManager.Infrastructure;

namespace StudioManager.Application.Users.Create;

public sealed class CreateUserCommandHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory,
    IKeyCloakService keyCloakService)
    : ICommandHandler<CreateUserCommand>
{
    public async Task<CommandResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.Create(request.User.FirstName, request.User.LastName, request.User.Email);
        await keyCloakService.AddUserAsync(user, cancellationToken);

        var identityUser = await keyCloakService.GetIdentityUserByEmail(user.Email, cancellationToken);

        if (identityUser is null)
        {
            return CommandResult.NotFound<UserRepresentation>();
        }
        
        user.SetNewIdentityId(identityUser.Id);
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return CommandResult.Success(user.Id);
    }
}
