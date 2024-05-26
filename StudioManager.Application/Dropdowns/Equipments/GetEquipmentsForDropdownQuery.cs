using StudioManager.API.Contracts.Common;
using StudioManager.Domain.Common.Results;

namespace StudioManager.Application.Dropdowns.Equipments;

public sealed record GetEquipmentsForDropdownQuery(string? Search) : IQuery<IReadOnlyList<NamedBaseDto>>;
