using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.Equipments.Delete;

public sealed record DeleteEquipmentCommand(Guid Id) : ICommand;
