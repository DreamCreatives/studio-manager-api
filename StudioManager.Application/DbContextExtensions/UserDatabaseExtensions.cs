using Microsoft.EntityFrameworkCore;
using StudioManager.Domain.Entities;
using StudioManager.Domain.Filters;
using StudioManager.Domain.Filters.Builders;
using StudioManager.Infrastructure.Common;

namespace StudioManager.Application.DbContextExtensions;

public static class UserDatabaseExtensions
{
    public static async Task<bool> CheckIfSimilarExistsAsync(this DbContextBase dbContext,
        UserFilter filter,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .Where(filter.ToQuery())
            .AnyAsync(cancellationToken);
    }
    
    public static async Task<User?> GetUserByIdAsync(this DbContextBase dbContext,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var filter = UserFilterBuilder.New().WithId(id).Build();
        
        return await dbContext.Users
            .Where(filter.ToQuery())
            .FirstOrDefaultAsync(cancellationToken);
    }
}
