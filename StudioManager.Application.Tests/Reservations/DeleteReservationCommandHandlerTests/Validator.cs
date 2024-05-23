using FluentAssertions;
using NUnit.Framework;
using StudioManager.Application.Reservations.Delete;

namespace StudioManager.Application.Tests.Reservations.DeleteReservationCommandHandlerTests;

public sealed class Validator
{
    [Test]
    public async Task should_return_error_when_id_is_empty()
    {
        // Arrange
        var command = new DeleteReservationCommand(Guid.Empty);
        var validator = new DeleteReservationCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain(x =>
            x.PropertyName == nameof(DeleteReservationCommand.Id) && x.ErrorMessage == "'Id' must not be empty.");
    }
}
