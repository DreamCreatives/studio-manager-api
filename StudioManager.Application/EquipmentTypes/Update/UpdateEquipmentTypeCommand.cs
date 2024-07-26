using StudioManager.API.Contracts.EquipmentTypes;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.EquipmentTypes.Update;

public sealed record UpdateEquipmentTypeCommand(Guid Id, EquipmentTypeWriteDto EquipmentType) : ICommand;
