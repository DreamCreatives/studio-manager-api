using MediatR;
using StudioManager.API.Contracts.EquipmentTypes;
using StudioManager.Domain.Common.Results;

namespace StudioManager.Application.EquipmentTypes.Create;

public sealed record CreateEquipmentTypeCommand(EquipmentTypeWriteDto EquipmentType) : IRequest<CommandResult>;
