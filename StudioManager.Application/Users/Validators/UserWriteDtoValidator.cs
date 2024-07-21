using FluentValidation;
using StudioManager.API.Contracts.Users;

namespace StudioManager.Application.Users.Validators;

public sealed class UserWriteDtoValidator : AbstractValidator<UserWriteDto>
{
    public UserWriteDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50);
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50);
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }    
}
