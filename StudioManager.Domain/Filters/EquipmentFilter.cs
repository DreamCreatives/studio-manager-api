using System.Linq.Expressions;
using StudioManager.Domain.Entities;

namespace StudioManager.Domain.Filters;

public sealed class EquipmentFilter : IFilter<Equipment>
{
    public Guid? Id { get; init; }
    public Guid? NotId { get; init; }
    public Guid? EquipmentTypeId { get; init; }
    public IEnumerable<Guid> EquipmentTypeIds { get; init; } = [];
    public string? ExactName { get; init; }
    public string? Search { get; init; }
    public Expression<Func<Equipment, bool>> ToQuery()
    {
        var lowerSearch = Search?.ToLower();
        var lowerExactName = ExactName?.ToLower();

        return x =>
            (!Id.HasValue || x.Id == Id) &&
            (!NotId.HasValue || x.Id != NotId) &&
            (!EquipmentTypeId.HasValue || x.EquipmentTypeId == EquipmentTypeId) &&
            (lowerExactName == null || lowerExactName.Length == 0 || x.Name.ToLower() == lowerExactName) &&
            (lowerSearch == null || lowerSearch.Length == 0 ||
             x.Id.ToString().ToLower().Equals(lowerSearch) ||
             x.Name.ToLower().Contains(lowerSearch)) &&
            (!EquipmentTypeIds.Any() || EquipmentTypeIds.Contains(x.EquipmentTypeId));
    }
}