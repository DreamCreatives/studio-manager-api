using StudioManager.API.Contracts.Users;
using StudioManager.Domain.Common.Results;

namespace StudioManager.Application.Users.GetById;

public sealed record GetUserByIdQuery(Guid Id) : IQuery<UserReadDto>;
