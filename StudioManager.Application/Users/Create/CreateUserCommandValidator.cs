using FluentValidation;
using StudioManager.Application.Users.Validators;

namespace StudioManager.Application.Users.Create;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(UserWriteDtoValidator dtoValidator)
    {
        RuleFor(x => x.User).NotNull();
        RuleFor(x => x.User).SetValidator(dtoValidator);
    }
}
