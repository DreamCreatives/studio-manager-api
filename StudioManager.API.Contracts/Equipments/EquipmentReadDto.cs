using StudioManager.API.Contracts.EquipmentTypes;

namespace StudioManager.API.Contracts.Equipments;

public sealed record EquipmentReadDto(
    Guid Id,
    string Name,
    int Quantity,
    int InitialQuantity,
    string ImageUrl,
    EquipmentTypeReadDto EquipmentType);
