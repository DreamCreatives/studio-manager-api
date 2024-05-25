using FluentAssertions;
using NUnit.Framework;
using StudioManager.Application.EquipmentTypes.GetById;

namespace StudioManager.Application.Tests.EquipmentTypes.GetEquipmentTypeByIdQueryHandlerTests;

public sealed class Validator
{
    [Test]
    public async Task should_return_error_when_id_is_empty_async()
    {
        // Arrange
        var query = new GetEquipmentTypeByIdQuery(Guid.Empty);
        var validator = new GetEquipmentTypeByIdQueryValidator();

        // Act
        var result = await validator.ValidateAsync(query, CancellationToken.None);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(query.Id) && e.ErrorMessage == "'Id' must not be empty.");
    }

    [Test]
    public async Task should_return_success_when_query_is_valid_async()
    {
        // Arrange
        var query = new GetEquipmentTypeByIdQuery(Guid.NewGuid());
        var validator = new GetEquipmentTypeByIdQueryValidator();

        // Act
        var result = await validator.ValidateAsync(query, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
