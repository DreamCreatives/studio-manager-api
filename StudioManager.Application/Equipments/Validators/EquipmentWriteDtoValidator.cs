using FluentValidation;
using StudioManager.API.Contracts.Equipments;

namespace StudioManager.Application.Equipments.Validators;

public sealed class EquipmentWriteDtoValidator : AbstractValidator<EquipmentWriteDto>
{
    public EquipmentWriteDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.EquipmentTypeId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(1);
        RuleFor(x => x.Quantity).LessThanOrEqualTo(int.MaxValue);
    }
}
