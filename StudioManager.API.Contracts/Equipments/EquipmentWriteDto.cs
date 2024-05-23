namespace StudioManager.API.Contracts.Equipments;

public sealed record EquipmentWriteDto(string Name, Guid EquipmentTypeId, int Quantity /*, string ImageUrl*/);
