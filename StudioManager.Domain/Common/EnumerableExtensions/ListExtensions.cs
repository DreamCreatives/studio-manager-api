using System.Linq.Expressions;
using LinqKit;

namespace StudioManager.Domain.Common.EnumerableExtensions;

public static class ListExtensions
{
    public static IReadOnlyList<TMember> MakeReadOnly<TMember>(this List<TMember> list) => list;
    
    public static Expression<Func<T, bool>> CombineExpressionsWithAnd<T>(this List<Expression<Func<T, bool>>> filters)
    {
        var combined = PredicateBuilder.New<T>(true);
        
        return filters.Aggregate(combined, (current, filter) => current.And(filter));
    }
}
