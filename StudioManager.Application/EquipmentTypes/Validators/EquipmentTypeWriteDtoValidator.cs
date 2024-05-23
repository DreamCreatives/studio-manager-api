using FluentValidation;
using StudioManager.API.Contracts.EquipmentTypes;

namespace StudioManager.Application.EquipmentTypes.Validators;

public sealed class EquipmentTypeWriteDtoValidator : AbstractValidator<EquipmentTypeWriteDto>
{
    public EquipmentTypeWriteDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}
