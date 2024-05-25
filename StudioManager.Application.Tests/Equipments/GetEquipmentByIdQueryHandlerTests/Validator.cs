using FluentAssertions;
using NUnit.Framework;
using StudioManager.Application.Equipments.GetById;

namespace StudioManager.Application.Tests.Equipments.GetEquipmentByIdQueryHandlerTests;

public sealed class Validator
{
    [Test]
    public async Task should_return_error_when_id_is_empty_async()
    {
        // Arrange
        var command = new GetEquipmentByIdQuery(Guid.Empty);
        var validator = new GetEquipmentByIdQueryValidator();

        // Act
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Errors.Should().HaveCount(1);
        result.Errors.First().ErrorMessage.Should().Be("'Id' must not be empty.");
    }
    
    [Test]
    public async Task should_return_success_when_query_is_valid_async()
    {
        // Arrange
        var command = new GetEquipmentByIdQuery(Guid.NewGuid());
        var validator = new GetEquipmentByIdQueryValidator();

        // Act
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
