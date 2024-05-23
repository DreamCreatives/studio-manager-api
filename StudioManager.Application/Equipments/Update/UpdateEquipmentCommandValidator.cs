using FluentValidation;
using StudioManager.Application.Equipments.Validators;

namespace StudioManager.Application.Equipments.Update;

public sealed class UpdateEquipmentCommandValidator : AbstractValidator<UpdateEquipmentCommand>
{
    public UpdateEquipmentCommandValidator(EquipmentWriteDtoValidator dtoValidator)
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Equipment).NotNull();
        RuleFor(x => x.Equipment).SetValidator(dtoValidator);
    }
}
