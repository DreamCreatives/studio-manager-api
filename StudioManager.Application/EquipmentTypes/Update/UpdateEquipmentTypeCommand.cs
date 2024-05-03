using MediatR;
using StudioManager.API.Contracts.EquipmentTypes;
using StudioManager.Domain.Common.Results;

namespace StudioManager.Application.EquipmentTypes.Update;

public sealed record UpdateEquipmentTypeCommand(Guid Id, EquipmentTypeWriteDto EquipmentType) : IRequest<CommandResult>;