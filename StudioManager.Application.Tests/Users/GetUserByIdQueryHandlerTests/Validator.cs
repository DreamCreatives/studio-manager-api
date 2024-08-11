using FluentValidation.TestHelper;
using NUnit.Framework;
using StudioManager.Application.Users.GetById;

namespace StudioManager.Application.Tests.Users.GetUserByIdQueryHandlerTests;

public sealed class Validator
{
    private static GetUserByIdQueryValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new GetUserByIdQueryValidator();
    }

    [Test]
    public void Validate_WithValidUserId_ShouldNotHaveValidationError()
    {
        // Arrange
        var query = new GetUserByIdQuery(Guid.NewGuid());
        
        // Act
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.Id);
    }

    [Test]
    public void Validate_WithEmptyUserId_ShouldHaveValidationError()
    {
        // Arrange
        var query = new GetUserByIdQuery(Guid.Empty);
        
        // Act
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Id);
    }
}
