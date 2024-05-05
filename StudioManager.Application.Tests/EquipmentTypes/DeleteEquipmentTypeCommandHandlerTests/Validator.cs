using FluentAssertions;
using NUnit.Framework;
using StudioManager.Application.EquipmentTypes.Delete;

namespace StudioManager.Application.Tests.EquipmentTypes.DeleteEquipmentTypeCommandHandlerTests;

public sealed class Validator
{
    [Test]
    public async Task validator_should_return_validation_error_when_name_is_empty()
    {
        // Arrange
        var command = new DeleteEquipmentTypeCommand(Guid.Empty);
        var validator = new DeleteEquipmentTypeCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Id" && x.ErrorMessage == "'Id' must not be empty.");
    }
    
    [Test]
    public async Task validator_should_return_success_when_name_is_not_empty()
    {
        // Arrange
        var command = new DeleteEquipmentTypeCommand(Guid.NewGuid());
        var validator = new DeleteEquipmentTypeCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}