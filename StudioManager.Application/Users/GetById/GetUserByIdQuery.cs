using StudioManager.API.Contracts.Users;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.Users.GetById;

public sealed record GetUserByIdQuery(Guid Id) : IQuery<UserReadDto>;
