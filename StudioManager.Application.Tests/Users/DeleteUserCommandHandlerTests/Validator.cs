using FluentValidation.TestHelper;
using NUnit.Framework;
using StudioManager.Application.Users.Delete;

namespace StudioManager.Application.Tests.Users.DeleteUserCommandHandlerTests;

public sealed class Validator
{
    [Test]
    public void DeleteUserCommandValidator_WithValidUserId_PassesValidation()
    {
        // Arrange
        var validator = new DeleteUserCommandValidator();
        var command = new DeleteUserCommand(Guid.NewGuid());

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Id);
    }

    [Test]
    public void DeleteUserCommandValidator_WithEmptyUserId_FailsValidation()
    {
        // Arrange
        var validator = new DeleteUserCommandValidator();
        var command = new DeleteUserCommand(Guid.Empty);

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id);
    }
}
