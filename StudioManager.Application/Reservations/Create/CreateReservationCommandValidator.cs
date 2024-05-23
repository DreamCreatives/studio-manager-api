using FluentValidation;
using StudioManager.Application.Reservations.Validators;

namespace StudioManager.Application.Reservations.Create;

public sealed class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
{
    public CreateReservationCommandValidator(ReservationWriteDtoValidator dtoValidator)
    {
        RuleFor(x => x.Reservation).NotNull();
        RuleFor(x => x.Reservation).SetValidator(dtoValidator);
    }
}
