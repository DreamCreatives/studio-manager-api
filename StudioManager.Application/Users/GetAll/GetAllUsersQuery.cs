using StudioManager.API.Contracts.Pagination;
using StudioManager.API.Contracts.Users;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Filters;

namespace StudioManager.Application.Users.GetAll;

public sealed record GetAllUsersQuery(UserFilter Filter, PaginationDto Pagination) : IQuery<PagingResultDto<UserReadDto>>;
