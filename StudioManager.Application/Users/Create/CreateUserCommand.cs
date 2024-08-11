using StudioManager.API.Contracts.Users;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.Users.Create;

public sealed record CreateUserCommand(UserWriteDto User) : ICommand;
