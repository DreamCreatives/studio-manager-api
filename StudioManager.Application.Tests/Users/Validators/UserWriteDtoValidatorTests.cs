using FluentValidation.TestHelper;
using NUnit.Framework;
using StudioManager.API.Contracts.Users;
using StudioManager.Application.Users.Validators;

namespace StudioManager.Application.Tests.Users.Validators;

[TestFixture]
public sealed class UserWriteDtoValidatorTests
{
    private UserWriteDtoValidator _validator;
    
    [SetUp]
    public void Setup()
    {
        _validator = new UserWriteDtoValidator();
    }

    [Test]
    public void FirstName_WhenNotEmptyAndMaxLength50_IsValid()
    {
        var model = new UserWriteDto("John", "Doe", "email@example.com");
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(user => user.FirstName);
    }

    [Test]
    public void FirstName_WhenEmpty_HasValidationError()
    {
        var model = new UserWriteDto(string.Empty, "Doe", "email@example.com");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(user => user.FirstName);
    }

    [Test]
    public void FirstName_WhenExceedsMaxLength_HasValidationError()
    {
        var model = new UserWriteDto(new string('a', 51), "Doe", "email@example.com");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(user => user.FirstName);
    }

    [Test]
    public void LastName_WhenNotEmptyAndMaxLength50_IsValid()
    {
        var model = new UserWriteDto("John", "Doe", "email@example.com");
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(user => user.LastName);
    }

    [Test]
    public void LastName_WhenEmpty_HasValidationError()
    {
        var model = new UserWriteDto("John", string.Empty, "email@example.com");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(user => user.LastName);
    }

    [Test]
    public void LastName_WhenExceedsMaxLength_HasValidationError()
    {
        var model = new UserWriteDto("John", new string('b', 51), "email@example.com");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(user => user.LastName);
    }

    [Test]
    public void Email_WhenNotEmptyAndValid_IsValid()
    {
        var model = new UserWriteDto("John", "Doe", "email@example.com");
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(user => user.Email);
    }

    [Test]
    public void Email_WhenEmpty_HasValidationError()
    {
        var model = new UserWriteDto("John", "Doe", string.Empty);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(user => user.Email);
    }

    [Test]
    public void Email_WhenInvalid_HasValidationError()
    {
        var model = new UserWriteDto("John", "Doe", "invalid_email");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(user => user.Email);
    }
}
