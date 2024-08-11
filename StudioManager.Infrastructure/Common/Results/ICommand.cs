using MediatR;

namespace StudioManager.Infrastructure.Common.Results;

public interface ICommand : IRequest<CommandResult>;
