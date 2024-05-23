using MediatR;
using StudioManager.API.Contracts.Equipments;
using StudioManager.Domain.Common.Results;

namespace StudioManager.Application.Equipments.Update;

public sealed record UpdateEquipmentCommand(Guid Id, EquipmentWriteDto Equipment) : IRequest<CommandResult>;
