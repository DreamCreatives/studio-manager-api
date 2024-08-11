using StudioManager.API.Contracts.Equipments;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.Equipments.Create;

public sealed record CreateEquipmentCommand(EquipmentWriteDto Equipment) : ICommand;
