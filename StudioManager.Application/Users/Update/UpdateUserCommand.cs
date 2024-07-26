using StudioManager.API.Contracts.Users;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.Users.Update;

public sealed record UpdateUserCommand(Guid Id, UserWriteDto User) : ICommand;
