using FluentValidation;

namespace StudioManager.Application.Reservations.GetById;

public sealed class GetReservationByIdQueryValidator : AbstractValidator<GetReservationByIdQuery>
{
    public GetReservationByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
