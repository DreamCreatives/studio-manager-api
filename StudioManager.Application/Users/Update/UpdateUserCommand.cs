using StudioManager.API.Contracts.Users;
using StudioManager.Domain.Common.Results;

namespace StudioManager.Application.Users.Update;

public sealed record UpdateUserCommand(Guid Id, UserWriteDto User) : ICommand;
