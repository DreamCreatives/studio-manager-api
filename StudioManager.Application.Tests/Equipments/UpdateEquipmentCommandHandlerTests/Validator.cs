using FluentAssertions;
using NUnit.Framework;
using StudioManager.API.Contracts.Equipments;
using StudioManager.Application.Equipments.Update;
using StudioManager.Application.Equipments.Validators;

namespace StudioManager.Application.Tests.Equipments.UpdateEquipmentCommandHandlerTests;

public sealed class Validator
{
    private const string Name = "Validation-Name";
    private static readonly Guid EquipmentTypeId = Guid.NewGuid();
    private const int Quantity = 1;
    
    [Test]
    public async Task should_return_error_when_id_is_empty_async()
    {
        var validator = new UpdateEquipmentCommandValidator(new EquipmentWriteDtoValidator());
        var command = new UpdateEquipmentCommand(Guid.Empty, new EquipmentWriteDto(Name, EquipmentTypeId, Quantity));
        var result = await validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(command.Id) && x.ErrorMessage == "'Id' must not be empty.");
    }
    
    [Test]
    public async Task should_return_error_when_equipment_is_null_async()
    {
        var validator = new UpdateEquipmentCommandValidator(new EquipmentWriteDtoValidator());
        var command = new UpdateEquipmentCommand(EquipmentTypeId, null!);
        var result = await validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(command.Equipment) && x.ErrorMessage == "'Equipment' must not be empty.");
    }
    
    [Test]
    public async Task should_return_error_when_equipment_model_is_invalid_async()
    {
        var validator = new UpdateEquipmentCommandValidator(new EquipmentWriteDtoValidator());
        var command = new UpdateEquipmentCommand(EquipmentTypeId, new EquipmentWriteDto(string.Empty, Guid.Empty, 0));
        var result = await validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Equipment.Name" && x.ErrorMessage == "'Name' must not be empty.");
        result.Errors.Should().Contain(x => x.PropertyName == "Equipment.EquipmentTypeId" && x.ErrorMessage == "'Equipment Type Id' must not be empty.");
        result.Errors.Should().Contain(x => x.PropertyName == "Equipment.Quantity" && x.ErrorMessage == "'Quantity' must be greater than or equal to '1'.");
    }
}