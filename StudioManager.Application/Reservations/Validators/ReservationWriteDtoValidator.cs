using FluentValidation;
using StudioManager.API.Contracts.Reservations;

namespace StudioManager.Application.Reservations.Validators;

public sealed class ReservationWriteDtoValidator : AbstractValidator<ReservationWriteDto>
{
    public ReservationWriteDtoValidator()
    {
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.StartDate).LessThan(x => x.EndDate);
        RuleFor(x => x.StartDate).GreaterThan(DateOnly.FromDateTime(DateTime.UtcNow));

        RuleFor(x => x.EndDate).NotEmpty();
        RuleFor(x => x.EndDate).GreaterThan(DateOnly.FromDateTime(DateTime.UtcNow));
        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);

        RuleFor(x => x.EquipmentId).NotEmpty();

        RuleFor(x => x.Quantity).NotNull();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.Quantity).LessThanOrEqualTo(int.MaxValue);
    }
}
