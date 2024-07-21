using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StudioManager.API.Contracts.Users;
using StudioManager.Domain.Common.Results;
using StudioManager.Infrastructure;

namespace StudioManager.Application.Users.GetById;

public sealed class GetUserByIdQueryHandler(
    IDbContextFactory<StudioManagerDbContext> dbContextFactory,
    IMapper mapper)
    : IQueryHandler<GetUserByIdQuery, UserReadDto>
{
    public async Task<QueryResult<UserReadDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var user = await dbContext.Users
            .Where(x => x.Id == request.Id)
            .ProjectTo<UserReadDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
        
        return user is not null
            ? QueryResult.Success(user)
            : QueryResult<UserReadDto>.NotFound(request.Id);
    }
}
