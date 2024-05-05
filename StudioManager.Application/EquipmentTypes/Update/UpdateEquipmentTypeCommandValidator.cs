using FluentValidation;
using StudioManager.Application.EquipmentTypes.Validators;

namespace StudioManager.Application.EquipmentTypes.Update;

public sealed class UpdateEquipmentTypeCommandValidator : AbstractValidator<UpdateEquipmentTypeCommand>
{
    public UpdateEquipmentTypeCommandValidator(EquipmentTypeWriteDtoValidator dtoValidator)
    {
        RuleFor(x => x.EquipmentType).SetValidator(dtoValidator);
        RuleFor(x => x.Id).NotEmpty();
    }
}