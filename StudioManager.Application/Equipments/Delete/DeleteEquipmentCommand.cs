using MediatR;
using StudioManager.Domain.Common.Results;

namespace StudioManager.Application.Equipments.Delete;

public sealed record DeleteEquipmentCommand(Guid Id) : IRequest<CommandResult>;