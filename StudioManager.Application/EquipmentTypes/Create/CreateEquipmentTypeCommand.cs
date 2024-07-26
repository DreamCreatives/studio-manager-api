using StudioManager.API.Contracts.EquipmentTypes;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.EquipmentTypes.Create;

public sealed record CreateEquipmentTypeCommand(EquipmentTypeWriteDto EquipmentType) : ICommand;
