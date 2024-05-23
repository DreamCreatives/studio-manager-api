using FluentAssertions;
using NUnit.Framework;
using StudioManager.API.Contracts.EquipmentTypes;
using StudioManager.Application.EquipmentTypes.Update;
using StudioManager.Application.EquipmentTypes.Validators;

namespace StudioManager.Application.Tests.EquipmentTypes.UpdateEquipmentTypeCommandHandlerTests;

public sealed class Validator
{
    [Test]
    public async Task validator_should_return_validation_errors_when_model_is_invalid()
    {
        // Arrange
        var command = new UpdateEquipmentTypeCommand(Guid.Empty, new EquipmentTypeWriteDto(string.Empty));
        var dtoValidator = new EquipmentTypeWriteDtoValidator();
        var validator = new UpdateEquipmentTypeCommandValidator(dtoValidator);

        // Act
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Id" && x.ErrorMessage == "'Id' must not be empty.");
        result.Errors.Should().Contain(x =>
            x.PropertyName == "EquipmentType.Name" && x.ErrorMessage == "'Name' must not be empty.");
    }

    [Test]
    public async Task validator_should_return_validation_error_when_id_is_empty()
    {
        // Arrange
        var command = new UpdateEquipmentTypeCommand(Guid.Empty, new EquipmentTypeWriteDto("Test-Equipment-Type"));
        var dtoValidator = new EquipmentTypeWriteDtoValidator();
        var validator = new UpdateEquipmentTypeCommandValidator(dtoValidator);

        // Act
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Id" && x.ErrorMessage == "'Id' must not be empty.");
    }

    [Test]
    public async Task validator_should_return_validation_error_when_name_is_empty()
    {
        // Arrange
        var command = new UpdateEquipmentTypeCommand(Guid.NewGuid(), new EquipmentTypeWriteDto(string.Empty));
        var dtoValidator = new EquipmentTypeWriteDtoValidator();
        var validator = new UpdateEquipmentTypeCommandValidator(dtoValidator);

        // Act
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x =>
            x.PropertyName == "EquipmentType.Name" && x.ErrorMessage == "'Name' must not be empty.");
    }

    [Test]
    public async Task validator_should_return_success_when_name_is_not_empty()
    {
        // Arrange
        var command = new UpdateEquipmentTypeCommand(Guid.NewGuid(), new EquipmentTypeWriteDto("Test-Equipment-Type"));
        var dtoValidator = new EquipmentTypeWriteDtoValidator();
        var validator = new UpdateEquipmentTypeCommandValidator(dtoValidator);

        // Act
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
