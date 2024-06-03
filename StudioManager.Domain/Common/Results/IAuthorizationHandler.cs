namespace StudioManager.Domain.Common.Results;

public interface IAuthorizationHandler<in TCommand>
{
    Task<CommandResult> AuthorizeAsync(TCommand command, CancellationToken cancellationToken = default);
}
