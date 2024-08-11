using FluentValidation.TestHelper;
using NUnit.Framework;
using StudioManager.API.Contracts.Users;
using StudioManager.Application.Users.Create;
using StudioManager.Application.Users.Validators;

namespace StudioManager.Application.Tests.Users.CreateUserCommandHandlerTests;

[TestFixture]
public sealed class CreateUserCommandValidatorTests
{
    private CreateUserCommandValidator _validator;
    private UserWriteDtoValidator _dtoValidator;

    [SetUp]
    public void Setup()
    {
        _dtoValidator = new UserWriteDtoValidator();
        _validator = new CreateUserCommandValidator(_dtoValidator);
    }

    [Test]
    public void CreateUserCommand_WithValidUser_PassesValidation()
    {
        var command = new CreateUserCommand(new UserWriteDto("John", "Doe", "john.doe@example.com"));
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void CreateUserCommand_WithNullUser_FailsValidation()
    {
        var command = new CreateUserCommand(null!);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.User);
    }

    [Test]
    public void CreateUserCommand_WithInvalidUser_FailsValidation()
    {
        var command = new CreateUserCommand(new UserWriteDto("John", "Doe", ""));
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.User.Email);
    }
}