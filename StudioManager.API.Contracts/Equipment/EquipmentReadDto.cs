using StudioManager.API.Contracts.EquipmentTypes;

namespace StudioManager.API.Contracts.Equipment;

public sealed record EquipmentReadDto(Guid Id, string Name, int Quantity, string ImageUrl, EquipmentTypeReadDto EquipmentType);