using FS.Keycloak.RestApiClient.Model;
using Microsoft.EntityFrameworkCore;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Application.KeyCloak;
using StudioManager.Domain.Entities;
using StudioManager.Domain.Filters.Builders;
using StudioManager.Infrastructure;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.Users.Create;

public sealed class CreateUserCommandHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory,
    IKeyCloakService keyCloakService)
    : ICommandHandler<CreateUserCommand>
{
    public async Task<CommandResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        
        var hasDuplicateEmail = await CheckEmailUniquenessAsync(dbContext, request.User.Email, cancellationToken);

        if (hasDuplicateEmail)
        {
            return CommandResult.Conflict($"User with email {request.User.Email} already exists.");
        }

        var user = User.Create(request.User.FirstName, request.User.LastName, request.User.Email);
        var keycloakResult = await keyCloakService.AddUserAsync(user, cancellationToken);

        if (keycloakResult.Succeeded is false)
        {
            return keycloakResult;
        }

        var identityUser = await keyCloakService.GetIdentityUserByEmail(user.Email, cancellationToken);

        if (identityUser is null)
        {
            return CommandResult.NotFound<UserRepresentation>();
        }

        user.SetNewIdentityId(identityUser.Id);

        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return CommandResult.Success(user.Id);
    }

    private static async Task<bool> CheckEmailUniquenessAsync(StudioManagerDbContext dbContext, string email, CancellationToken cancellationToken)
    {
        var uniqueFilter = UserFilterBuilder.New()
            .WithEmail(email)
            .Build();        
        
        return await dbContext.CheckIfSimilarExistsAsync(uniqueFilter, cancellationToken);
    }
}
