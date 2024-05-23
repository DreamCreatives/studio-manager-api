using FluentValidation;

namespace StudioManager.Application.EquipmentTypes.Delete;

public sealed class DeleteEquipmentTypeCommandValidator : AbstractValidator<DeleteEquipmentTypeCommand>
{
    public DeleteEquipmentTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
