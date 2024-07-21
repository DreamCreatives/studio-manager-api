using Microsoft.EntityFrameworkCore;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Application.KeyCloak;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Entities;
using StudioManager.Domain.Filters.Builders;
using StudioManager.Infrastructure;

namespace StudioManager.Application.Users.Update;

public sealed class UpdateUserCommandHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory,
    IKeyCloakService keyCloakService)
    : ICommandHandler<UpdateUserCommand>
{
    public async Task<CommandResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var exists = await dbContext.CheckIfSimilarExistsAsync(
            UserFilterBuilder.New()
                .WithEmail(request.User.Email)
                .WithNotId(request.Id)
                .Build(), 
            cancellationToken);

        if (exists)
        {
            return CommandResult.Conflict($"User with email {request.User.Email} already exists");
        }

        var user = await dbContext.GetUserByIdAsync(request.Id, cancellationToken);

        if (user is null)
        {
            return CommandResult.NotFound<User>(request.Id);
        }

        var result = await keyCloakService.UpdateUserAsync(user, cancellationToken);

        if (!result.Succeeded) return result;
        
        user.Update(request.User.FirstName, request.User.LastName, request.User.Email);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return CommandResult.Success();
    }
}
