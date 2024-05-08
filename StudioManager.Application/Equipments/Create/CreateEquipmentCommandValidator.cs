using FluentValidation;
using StudioManager.Application.Equipments.Validators;

namespace StudioManager.Application.Equipments.Create;

public sealed class CreateEquipmentCommandValidator : AbstractValidator<CreateEquipmentCommand>
{
    public CreateEquipmentCommandValidator(EquipmentWriteDtoValidator dtoValidator)
    {
        RuleFor(x => x.Equipment).SetValidator(dtoValidator);
    }
}