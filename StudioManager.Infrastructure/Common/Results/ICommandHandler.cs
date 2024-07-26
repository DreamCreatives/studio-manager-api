using MediatR;

namespace StudioManager.Infrastructure.Common.Results;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, CommandResult>
    where TCommand : ICommand;
