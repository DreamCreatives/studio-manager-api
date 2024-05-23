using System.Linq.Expressions;

namespace StudioManager.Domain.Filters;

public interface IFilter<T>
{
    Expression<Func<T, bool>> ToQuery();
}
