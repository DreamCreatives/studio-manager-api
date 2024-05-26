using MediatR;

namespace StudioManager.Domain.Common.Results;

public interface ICommand : IRequest<CommandResult>;
