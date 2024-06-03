using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;
using StudioManager.API.Contracts.Reservations;
using StudioManager.Domain.Entities;
using StudioManager.Tests.Common.AutoMapperExtensions;

namespace StudioManager.Application.Tests.Mapps.Reservations;

[ExcludeFromCodeCoverage]
public sealed class ReservationProjectionTests
{
    [Test]
    public void should_map_reservation_to_reservation_read_dto()
    {
        // Arrange
        var startDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var equipment = Equipment.Create("Test Equipment", Guid.Empty, 10);
        var user = User.Create("test", "user", "test@user.com", Guid.NewGuid());
        var reservation = Reservation.Create(startDate, startDate, 10, equipment.Id, user.Id);
        reservation.GetType().GetProperty(nameof(Reservation.Equipment))!.SetValue(reservation, equipment);
        reservation.GetType().GetProperty(nameof(Reservation.User))!.SetValue(reservation, user);

        var mapper = MappingTestHelper.Mapper;

        // Act
        var result = mapper.Map<ReservationReadDto>(reservation);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Quantity.Should().Be(reservation.Quantity);
        result.StartDate.Should().Be(reservation.StartDate);
        result.EndDate.Should().Be(reservation.EndDate);
        result.Equipment.Should().NotBeNull();
        result.Equipment.Id.Should().Be(equipment.Id);
        result.Equipment.Name.Should().Be(equipment.Name);
    }
}
