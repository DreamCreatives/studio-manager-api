using System.Linq.Expressions;
using StudioManager.Domain.Common.EnumerableExtensions;
using StudioManager.Domain.Entities;

namespace StudioManager.Domain.Filters;

public sealed class UserFilter : IFilter<User>
{
    public Guid? Id { get; init; }
    public string? Search { get; init; }
    public string? Email { get; init; }
    public Guid? NotId { get; init; }
    
    public Expression<Func<User, bool>> ToQuery()
    {
        var filters = new List<Expression<Func<User, bool>>>
        {
            AddIdFilter(),
            AddSearchFilter(),
            AddEmailFilter(),
            AddNotIdFilter()
        };

        return filters.CombineExpressionsWithAnd();
    }
    
    private Expression<Func<User, bool>> AddIdFilter()
    {
        return x => !Id.HasValue || x.Id == Id;
    }
    
    private Expression<Func<User, bool>> AddNotIdFilter()
    {
        return x => !NotId.HasValue || x.Id != NotId;
    }
    
    private Expression<Func<User, bool>> AddEmailFilter()
    {
        var lowerEmail = Email?.ToLower();
        return x => lowerEmail == null || x.Email.ToLower().Equals(lowerEmail);
    }

    private Expression<Func<User, bool>> AddSearchFilter()
    {
        var lowerSearch = Search?.ToLower();
        return x =>
            lowerSearch == null ||
            x.FirstName.ToLower().Contains(lowerSearch) ||
            x.LastName.ToLower().Contains(lowerSearch) ||
            string.Join("", x.FirstName, x.LastName).Replace(" ", "").ToLower().Contains(lowerSearch.Replace(" ", "")) ||
            string.Join("", x.LastName, x.FirstName).Replace(" ", "").ToLower().Contains(lowerSearch.Replace(" ", "")) ||
            x.Email.ToLower().Equals(lowerSearch);
    }
}
