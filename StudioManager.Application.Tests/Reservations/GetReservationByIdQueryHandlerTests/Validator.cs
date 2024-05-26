using FluentAssertions;
using NUnit.Framework;
using StudioManager.Application.Reservations.GetById;

namespace StudioManager.Application.Tests.Reservations.GetReservationByIdQueryHandlerTests;

public sealed class Validator
{
    [Test]
    public async Task should_return_error_when_id_is_empty_async()
    {
        // Arrange
        var query = new GetReservationByIdQuery(Guid.Empty);
        var validator = new GetReservationByIdQueryValidator();

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
        var query = new GetReservationByIdQuery(Guid.NewGuid());
        var validator = new GetReservationByIdQueryValidator();
        
        // Act
        var result = await validator.ValidateAsync(query, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
