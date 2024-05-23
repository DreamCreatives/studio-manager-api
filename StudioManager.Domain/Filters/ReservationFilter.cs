using System.Linq.Expressions;
using StudioManager.Domain.Entities;

namespace StudioManager.Domain.Filters;

public sealed class ReservationFilter : IFilter<Reservation>
{
    public Guid? Id { get; init; }
    public Guid? NotId { get; init; }
    public DateOnly? StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public string? Search { get; init; }

    public DateOnly? MaxStartDate { get; init; }
    public DateOnly? MinEndDate { get; init; }

    public Expression<Func<Reservation, bool>> ToQuery()
    {
        var lowerSearch = Search?.ToLower();

        return x =>
            (!Id.HasValue || x.Id == Id) &&
            (!NotId.HasValue || x.Id != NotId) &&
            (lowerSearch == null || lowerSearch.Length == 0 ||
             x.Id.ToString().ToLower().Equals(lowerSearch) ||
             x.Equipment.Name.ToLower().Contains(lowerSearch)) &&
            (!StartDate.HasValue || x.StartDate >= StartDate) &&
            (!EndDate.HasValue || x.EndDate <= EndDate) &&
            (!MaxStartDate.HasValue || x.StartDate <= MaxStartDate) &&
            (!MinEndDate.HasValue || x.EndDate >= MinEndDate);
    }
}
