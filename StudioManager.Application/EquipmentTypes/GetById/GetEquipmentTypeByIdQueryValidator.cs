using FluentValidation;

namespace StudioManager.Application.EquipmentTypes.GetById;

public sealed class GetEquipmentTypeByIdQueryValidator : AbstractValidator<GetEquipmentTypeByIdQuery>
{
    public GetEquipmentTypeByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
