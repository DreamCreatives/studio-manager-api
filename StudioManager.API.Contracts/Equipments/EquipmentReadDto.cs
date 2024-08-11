using StudioManager.API.Contracts.EquipmentTypes;

namespace StudioManager.API.Contracts.Equipments;

public sealed record EquipmentReadDto(
    Guid Id,
    string Name,
    int InitialQuantity,
    string ImageUrl,
    EquipmentTypeReadDto EquipmentType);
