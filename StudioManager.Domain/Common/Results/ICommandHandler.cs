using MediatR;

namespace StudioManager.Domain.Common.Results;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, CommandResult>
    where TCommand : ICommand;
