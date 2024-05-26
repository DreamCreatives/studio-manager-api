using StudioManager.API.Contracts.Common;
using StudioManager.Domain.Common.Results;

namespace StudioManager.Application.Dropdowns.EquipmentTypes;

public sealed record GetEquipmentTypesForDropdownQuery(string? Search) : IQuery<IReadOnlyList<NamedBaseDto>>;
