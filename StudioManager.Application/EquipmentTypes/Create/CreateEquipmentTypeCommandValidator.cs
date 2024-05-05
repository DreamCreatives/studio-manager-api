using FluentValidation;
using StudioManager.Application.EquipmentTypes.Validators;

namespace StudioManager.Application.EquipmentTypes.Create;

public sealed class CreateEquipmentTypeCommandValidator : AbstractValidator<CreateEquipmentTypeCommand>
{
    public CreateEquipmentTypeCommandValidator(EquipmentTypeWriteDtoValidator dtoValidator)
    {
        RuleFor(x => x.EquipmentType).SetValidator(dtoValidator);
    }
}