using FluentValidation;

namespace StudioManager.Application.Reservations.BetById;

public sealed class GetReservationByIdQueryValidator : AbstractValidator<GetReservationByIdQuery>
{
    public GetReservationByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
