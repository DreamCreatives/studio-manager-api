using MediatR;
using StudioManager.API.Contracts.Equipments;
using StudioManager.Domain.Common.Results;

namespace StudioManager.Application.Equipments.Create;

public sealed record CreateEquipmentCommand(EquipmentWriteDto Equipment) : IRequest<CommandResult>;
