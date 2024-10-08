﻿using FluentAssertions;
using Moq;
using NUnit.Framework;
using StudioManager.Application.Common;
using StudioManager.Application.Reservations.Delete;
using StudioManager.Application.Tests.Reservations.Common;
using StudioManager.Domain.ErrorMessages;
using StudioManager.Infrastructure;
using StudioManager.Tests.Common;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Application.Tests.Reservations.DeleteReservationCommandHandlerTests;

public sealed class AuthorizeAsync : IntegrationTestBase
{
    private static readonly DateOnly ValidDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
    private static TestDbContextFactory<StudioManagerDbContext> _dbContextFactory = null!;
    private static DeleteReservationCommandAuthorizationHandler _testCandidate = null!;
    private static Mock<ITokenDecryptor> _tokenDecryptor = null!;

    [SetUp]
    public async Task SetUpAsync()
    {
        var connectionString = await DbMigrator.MigrateDbAsync();
        _dbContextFactory = new TestDbContextFactory<StudioManagerDbContext>(connectionString);
        _tokenDecryptor = new Mock<ITokenDecryptor>();
        _tokenDecryptor.Setup(x => x.UserId).Returns(null as string);
        _testCandidate = new DeleteReservationCommandAuthorizationHandler(_dbContextFactory, _tokenDecryptor.Object);
    }
    
    [Test]
    public async Task should_return_forbidden_when_user_id_is_not_a_guid()
    {
        // Arrange
        var command = new DeleteReservationCommand(Guid.NewGuid());

        // Act
        var result = await _testCandidate.AuthorizeAsync(command, Cts.Token);

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(ForbiddenStatusCode);
        result.Data.Should().BeNull();
        result.Error.Should().NotBeNullOrWhiteSpace();
        result.Error.Should().Be(EX.FORBIDDEN);
    }
    
    [Test]
    public async Task should_return_forbidden_when_reservation_not_found()
    {
        // Arrange
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var reservation =
            await ReservationTestHelper.AddReservationForDetailsAsync(dbContext, ValidDate, ValidDate.AddDays(1));
        _tokenDecryptor.Setup(x => x.UserId).Returns(Guid.NewGuid().ToString);

        var command = new DeleteReservationCommand(reservation.Id);

        // Act
        var result = await _testCandidate.AuthorizeAsync(command, Cts.Token);

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(ForbiddenStatusCode);
        result.Data.Should().BeNull();
        result.Error.Should().NotBeNullOrWhiteSpace();
        result.Error.Should().Be(EX.FORBIDDEN);
    }
    
    [Test]
    public async Task should_return_success_when_reservation_found()
    {
        // Arrange
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var reservation =
            await ReservationTestHelper.AddReservationForDetailsAsync(dbContext, ValidDate, ValidDate.AddDays(1));
        _tokenDecryptor.Setup(x => x.UserId).Returns(reservation.UserId.ToString);

        var command = new DeleteReservationCommand(reservation.Id);

        // Act
        var result = await _testCandidate.AuthorizeAsync(command, Cts.Token);

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(OkStatusCode);
        result.Data.Should().BeNull();
        result.Error.Should().BeNull();
    }
}
