using FluentValidation;
using StudioManager.Application.Reservations.Validators;

namespace StudioManager.Application.Reservations.Update;

public sealed class UpdateReservationCommandValidator : AbstractValidator<UpdateReservationCommand>
{
    public UpdateReservationCommandValidator(ReservationWriteDtoValidator dtoValidator)
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Reservation).NotNull();
        RuleFor(x => x.Reservation).SetValidator(dtoValidator);
    }
}