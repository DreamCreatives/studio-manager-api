using FluentValidation;

namespace StudioManager.Application.Reservations.Delete;

public sealed class DeleteReservationCommandValidator : AbstractValidator<DeleteReservationCommand>
{
    public DeleteReservationCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}