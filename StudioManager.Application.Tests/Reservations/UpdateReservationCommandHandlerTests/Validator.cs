using FluentAssertions;
using NUnit.Framework;
using StudioManager.API.Contracts.Reservations;
using StudioManager.Application.Reservations.Update;
using StudioManager.Application.Reservations.Validators;

namespace StudioManager.Application.Tests.Reservations.UpdateReservationCommandHandlerTests;

public sealed class Validator
{
    [Test]
    public async Task should_return_error_when_command_id_is_empty_invalid_async()
    {
        // Arrange
        var command = new UpdateReservationCommand(Guid.Empty, new ReservationWriteDto(
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
            1,
            Guid.NewGuid()));
        
        var validator = new UpdateReservationCommandValidator(new ReservationWriteDtoValidator());
        
        // Act
        var result = await validator.ValidateAsync(command);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(command.Id) && x.ErrorMessage == "'Id' must not be empty.");
    }
    
    [Test]
    public async Task should_return_error_when_command_reservation_is_invalid_async()
    {
        // Arrange
        var command = new UpdateReservationCommand(Guid.NewGuid(), null!);
        
        var validator = new UpdateReservationCommandValidator(new ReservationWriteDtoValidator());
        
        // Act
        var result = await validator.ValidateAsync(command);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(command.Reservation) && x.ErrorMessage == "'Reservation' must not be empty.");
    }
}