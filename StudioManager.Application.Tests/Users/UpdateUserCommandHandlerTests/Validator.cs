using FluentValidation.TestHelper;
using NUnit.Framework;
using StudioManager.API.Contracts.Users;
using StudioManager.Application.Users.Update;
using StudioManager.Application.Users.Validators;

namespace StudioManager.Application.Tests.Users.UpdateUserCommandHandlerTests;

public sealed class Validator
{
    private UpdateUserCommandValidator _validator;
    private UserWriteDtoValidator _userWriteDtoValidator;

    [SetUp]
    public void SetUp()
    {
        _userWriteDtoValidator = new UserWriteDtoValidator();
        _validator = new UpdateUserCommandValidator(_userWriteDtoValidator);
    }

    [Test]
    public void Validate_WithValidParameters_ShouldNotHaveValidationError()
    {
        var command = new UpdateUserCommand(Guid.NewGuid(), new UserWriteDto(Faker.Name.First(), Faker.Name.Last(), Faker.Internet.Email()));
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WithEmptyId_ShouldHaveValidationError()
    {
        var command = new UpdateUserCommand(Guid.Empty, new UserWriteDto(Faker.Name.First(), Faker.Name.Last(), Faker.Internet.Email()));
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Id);
    }

    [Test]
    public void Validate_WithNullUser_ShouldHaveValidationError()
    {
        var command = new UpdateUserCommand(Guid.NewGuid(), null!);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.User);
    }

    [Test]
    public void Validate_WithInvalidUserFields_ShouldHaveValidationError()
    {
        var command = new UpdateUserCommand(Guid.NewGuid(), new UserWriteDto(string.Empty, string.Empty, string.Empty));
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.User.FirstName);
        result.ShouldHaveValidationErrorFor(c => c.User.LastName);
        result.ShouldHaveValidationErrorFor(c => c.User.Email);
    }
}
