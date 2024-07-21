using FluentValidation;
using StudioManager.Application.Users.Validators;

namespace StudioManager.Application.Users.Update;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator(UserWriteDtoValidator dtoValidator)
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        
        RuleFor(x => x.User)
            .NotNull()
            .SetValidator(dtoValidator);
    }
}
