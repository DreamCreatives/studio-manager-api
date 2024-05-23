using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using StudioManager.Domain.Entities;

namespace StudioManager.Domain.Filters;

[SuppressMessage("Performance",
    "CA1862:Use the \'StringComparison\' method overloads to perform case-insensitive string comparisons")]
public sealed class EquipmentTypeFilter : IFilter<EquipmentType>
{
    public Guid? Id { get; init; }
    public Guid? NotId { get; init; }
    public string? Name { get; init; }
    public string? ExactName { get; init; }
    public string? Search { get; init; }

    public Expression<Func<EquipmentType, bool>> ToQuery()
    {
        var lowerSearch = Search?.ToLower();
        var lowerName = Name?.ToLower();
        var lowerExactName = ExactName?.ToLower();

        return equipmentType =>
            (!Id.HasValue || equipmentType.Id == Id) &&
            (!NotId.HasValue || equipmentType.Id != NotId) &&
            (lowerExactName == null || lowerExactName.Length == 0 ||
             equipmentType.Name.ToLower().Equals(lowerExactName)) &&
            (lowerName == null || lowerName.Length == 0 || equipmentType.Name.ToLower().Equals(lowerName)) &&
            (lowerSearch == null || lowerSearch.Length == 0 ||
             equipmentType.Id.ToString().ToLower().Equals(lowerSearch) ||
             equipmentType.Name.ToLower().Contains(lowerSearch));
    }
}
