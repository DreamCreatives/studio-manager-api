using StudioManager.API.Contracts.Pagination;
using StudioManager.API.Contracts.Users;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.Users.GetAll;

public sealed record GetAllUsersQuery(UserFilter Filter, PaginationDto Pagination) : IQuery<PagingResultDto<UserReadDto>>;
