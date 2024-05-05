namespace StudioManager.API.Contracts.Equipment;

public sealed record EquipmentWriteDto(string Name, Guid EquipmentTypeId, int Quantity/*, byte[] Image*/);