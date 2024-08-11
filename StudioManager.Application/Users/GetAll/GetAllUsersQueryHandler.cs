using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StudioManager.API.Contracts.Pagination;
using StudioManager.API.Contracts.Users;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Infrastructure;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.Users.GetAll;

public sealed class GetAllUsersQueryHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory,
    IMapper mapper)
    : IQueryHandler<GetAllUsersQuery, PagingResultDto<UserReadDto>>
{
    public async Task<QueryResult<PagingResultDto<UserReadDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var data = dbContext.Users
            .AsNoTracking()
            .Where(request.Filter.ToQuery())
            .ProjectTo<UserReadDto>(mapper.ConfigurationProvider);

        return await data.ApplyPagingAsync(request.Pagination);
    }
}
