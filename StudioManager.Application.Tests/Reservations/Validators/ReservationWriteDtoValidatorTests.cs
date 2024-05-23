using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;
using StudioManager.API.Contracts.Reservations;
using StudioManager.Application.Reservations.Validators;

namespace StudioManager.Application.Tests.Reservations.Validators;

[ExcludeFromCodeCoverage]
public sealed class ReservationWriteDtoValidatorTests
{
    private static readonly DateOnly ValidDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
    private static readonly DateOnly InvalidDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
    private static readonly DateOnly Today = DateOnly.FromDateTime(DateTime.UtcNow);
    private static readonly DateOnly EmptyDate = DateOnly.MinValue;

    [Test]
    public async Task should_return_error_when_start_date_is_lower_than_today_async()
    {
        // Arrange
        var dto = new ReservationWriteDto(InvalidDate, ValidDate, 1, Guid.NewGuid());
        var validator = new ReservationWriteDtoValidator();

        // Act
        var result = await validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain(x =>
            x.PropertyName == nameof(ReservationWriteDto.StartDate) &&
            x.ErrorMessage == $"'Start Date' must be greater than '{Today}'.");
    }

    [Test]
    public async Task should_return_error_when_start_date_is_empty_async()
    {
        // Arrange
        var dto = new ReservationWriteDto(EmptyDate, ValidDate, 1, Guid.NewGuid());
        var validator = new ReservationWriteDtoValidator();

        // Act
        var result = await validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(x =>
            x.PropertyName == nameof(ReservationWriteDto.StartDate) &&
            x.ErrorMessage == "'Start Date' must not be empty.");
    }

    [Test]
    public async Task should_return_error_when_start_date_is_greater_than_end_day_async()
    {
        // Arrange
        var dto = new ReservationWriteDto(ValidDate.AddDays(1), ValidDate, 1, Guid.NewGuid());
        var validator = new ReservationWriteDtoValidator();

        // Act
        var result = await validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(x =>
            x.PropertyName == nameof(ReservationWriteDto.StartDate) &&
            x.ErrorMessage == $"'Start Date' must be less than '{ValidDate}'.");
    }

    [Test]
    public async Task should_return_error_when_end_date_is_lower_than_today_async()
    {
        // Arrange
        var dto = new ReservationWriteDto(InvalidDate, Today.AddDays(-1), 1, Guid.NewGuid());
        var validator = new ReservationWriteDtoValidator();

        // Act
        var result = await validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(x =>
            x.PropertyName == nameof(ReservationWriteDto.EndDate) &&
            x.ErrorMessage == $"'End Date' must be greater than '{Today}'.");
    }

    [Test]
    public async Task should_return_error_when_end_date_is_empty_async()
    {
        // Arrange
        var dto = new ReservationWriteDto(EmptyDate, EmptyDate, 1, Guid.NewGuid());
        var validator = new ReservationWriteDtoValidator();

        // Act
        var result = await validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(x =>
            x.PropertyName == nameof(ReservationWriteDto.EndDate) && x.ErrorMessage == "'End Date' must not be empty.");
    }

    [Test]
    public async Task should_return_error_when_end_date_is_lower_than_end_day_async()
    {
        // Arrange
        var dto = new ReservationWriteDto(ValidDate.AddDays(1), ValidDate, 1, Guid.NewGuid());
        var validator = new ReservationWriteDtoValidator();

        // Act
        var result = await validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(x =>
            x.PropertyName == nameof(ReservationWriteDto.EndDate) &&
            x.ErrorMessage == $"'End Date' must be greater than '{ValidDate.AddDays(1)}'.");
    }

    [Test]
    public async Task should_return_error_when_equipmentId_is_empty_async()
    {
        // Arrange
        var dto = new ReservationWriteDto(ValidDate, ValidDate.AddDays(1), 1, Guid.Empty);
        var validator = new ReservationWriteDtoValidator();

        // Act
        var result = await validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(x =>
            x.PropertyName == nameof(ReservationWriteDto.EquipmentId) &&
            x.ErrorMessage == "'Equipment Id' must not be empty.");
    }

    [Test]
    public async Task should_return_error_when_quantity_is_zero_async()
    {
        // Arrange
        var dto = new ReservationWriteDto(ValidDate, ValidDate.AddDays(1), 0, Guid.NewGuid());
        var validator = new ReservationWriteDtoValidator();

        // Act
        var result = await validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(x =>
            x.PropertyName == nameof(ReservationWriteDto.Quantity) &&
            x.ErrorMessage == "'Quantity' must be greater than '0'.");
    }
}
