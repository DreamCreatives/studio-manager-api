using FluentValidation;

namespace StudioManager.Application.Equipments.Delete;

public sealed class DeleteEquipmentCommandValidator : AbstractValidator<DeleteEquipmentCommand>
{
    public DeleteEquipmentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
