using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;
using StudioManager.Application.Reservations.Create;
using StudioManager.Application.Reservations.Validators;

namespace StudioManager.Application.Tests.Reservations.CreateReservationCommandHandlerTests;

[ExcludeFromCodeCoverage]
public sealed class Validator
{
    [Test]
    public async Task should_return_error_when_reservation_is_null_async()
    {
        // Arrange
        var command = new CreateReservationCommand(null!);
        var validator = new CreateReservationCommandValidator(new ReservationWriteDtoValidator());
        
        // Act
        var result = await validator.ValidateAsync(command);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain(x => x.PropertyName == "Reservation" && x.ErrorMessage == "'Reservation' must not be empty.");
    }
}