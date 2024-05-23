using FluentAssertions;
using NUnit.Framework;
using StudioManager.API.Contracts.EquipmentTypes;
using StudioManager.Application.EquipmentTypes.Create;
using StudioManager.Application.EquipmentTypes.Validators;

namespace StudioManager.Application.Tests.EquipmentTypes.CreateEquipmentTypeCommandHandlerTests;

public sealed class Validator
{
    [Test]
    public async Task validator_should_return_validation_error_when_name_is_empty()
    {
        // Arrange
        var command = new CreateEquipmentTypeCommand(new EquipmentTypeWriteDto(string.Empty));
        var dtoValidator = new EquipmentTypeWriteDtoValidator();
        var validator = new CreateEquipmentTypeCommandValidator(dtoValidator);

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
        var command = new CreateEquipmentTypeCommand(new EquipmentTypeWriteDto("Name"));
        var dtoValidator = new EquipmentTypeWriteDtoValidator();
        var validator = new CreateEquipmentTypeCommandValidator(dtoValidator);

        // Act
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
