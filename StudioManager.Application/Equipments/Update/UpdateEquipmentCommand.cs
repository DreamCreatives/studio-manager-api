using StudioManager.API.Contracts.Equipments;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.Equipments.Update;

public sealed record UpdateEquipmentCommand(Guid Id, EquipmentWriteDto Equipment) : ICommand;
