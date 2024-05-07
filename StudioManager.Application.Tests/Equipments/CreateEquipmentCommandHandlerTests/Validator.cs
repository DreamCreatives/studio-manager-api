using FluentAssertions;
using NUnit.Framework;
using StudioManager.API.Contracts.Equipments;
using StudioManager.Application.Equipments.Create;
using StudioManager.Application.Equipments.Validators;

namespace StudioManager.Application.Tests.Equipments.CreateEquipmentCommandHandlerTests;

public sealed class Validator
{
    [Test]
    public async Task validator_should_return_validation_errors_when_model_is_invalid()
    {
        // Arrange
        var command = new CreateEquipmentCommand(new EquipmentWriteDto(string.Empty, Guid.Empty, 0));
        var dtoValidator = new EquipmentWriteDtoValidator();
        var validator = new CreateEquipmentCommandValidator(dtoValidator);

        // Act
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Equipment.Quantity" && x.ErrorMessage == "'Quantity' must be greater than or equal to '1'.");
        result.Errors.Should().Contain(x => x.PropertyName == "Equipment.EquipmentTypeId" && x.ErrorMessage == "'Equipment Type Id' must not be empty.");
        result.Errors.Should().Contain(x => x.PropertyName == "Equipment.Name" && x.ErrorMessage == "'Name' must not be empty.");
    }
    
    [Test]
    public async Task validator_should_return_validation_error_when_quantity_is_lower_than_1()
    {
        // Arrange
        var command = new CreateEquipmentCommand(new EquipmentWriteDto("Name", Guid.NewGuid(), 0));
        var dtoValidator = new EquipmentWriteDtoValidator();
        var validator = new CreateEquipmentCommandValidator(dtoValidator);

        // Act
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Equipment.Quantity" && x.ErrorMessage == "'Quantity' must be greater than or equal to '1'.");
    }
    
    [Test]
    public async Task validator_should_return_validation_error_when_equipment_type_id_is_empty()
    {
        // Arrange
        var command = new CreateEquipmentCommand(new EquipmentWriteDto("Name", Guid.Empty, 1));
        var dtoValidator = new EquipmentWriteDtoValidator();
        var validator = new CreateEquipmentCommandValidator(dtoValidator);

        // Act
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Equipment.EquipmentTypeId" && x.ErrorMessage == "'Equipment Type Id' must not be empty.");
    }
    
    [Test]
    public async Task validator_should_return_validation_error_when_name_is_empty()
    {
        // Arrange
        var command = new CreateEquipmentCommand(new EquipmentWriteDto(string.Empty, Guid.NewGuid(), 1));
        var dtoValidator = new EquipmentWriteDtoValidator();
        var validator = new CreateEquipmentCommandValidator(dtoValidator);

        // Act
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Equipment.Name" && x.ErrorMessage == "'Name' must not be empty.");
    }
    
    [Test]
    public async Task validator_should_return_success_when_model_is_valid()
    {
        // Arrange
        var command = new CreateEquipmentCommand(new EquipmentWriteDto("Name", Guid.NewGuid(), 1));
        var dtoValidator = new EquipmentWriteDtoValidator();
        var validator = new CreateEquipmentCommandValidator(dtoValidator);

        // Act
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}