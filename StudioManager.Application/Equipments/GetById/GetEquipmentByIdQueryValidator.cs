using FluentValidation;

namespace StudioManager.Application.Equipments.GetById;

public sealed class GetEquipmentByIdQueryValidator : AbstractValidator<GetEquipmentByIdQuery>
{
    public GetEquipmentByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
