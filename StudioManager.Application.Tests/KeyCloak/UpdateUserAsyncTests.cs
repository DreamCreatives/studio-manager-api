using System.Net;
using FluentAssertions;
using FS.Keycloak.RestApiClient.Model;
using NUnit.Framework;
using StudioManager.Domain.Entities;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.Tests.KeyCloak;

public sealed class UpdateUserAsyncTests : KeyCloakServiceTestBase
{
    [Test]
    public async Task Should_return_fail_when_getting_user_failed()
    {
        // Arrange
        UsersApiMock.Setup(x => x.GetUsersByUserIdAsync(
                It.IsAny<string>(), It.IsAny<string>(),
                null, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());
        
        // Act
        var result = await TestCandidate.UpdateUserAsync(new User(), CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CommandResult>();
        result.Data.Should().BeNull();
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    [Ignore("Required keycloak to work")]
    public async Task Should_return_fail_when_updating_user_throws()
    {
        // Arrange
        UsersApiMock.Setup(x => x.GetUsersByUserIdAsync(
                It.IsAny<string>(), It.IsAny<string>(),
                null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserRepresentation());
        
        UsersApiMock.Setup(x => x.PutUsersByUserIdAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserRepresentation>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await TestCandidate.UpdateUserAsync(new User(), CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CommandResult>();
        result.Data.Should().BeNull();
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
    
    [Test]
    [Ignore("Required keycloak to work")]
    public async Task Should_return_success()
    {
        // Arrange
        UsersApiMock.Setup(x => x.GetUsersByUserIdAsync(
                It.IsAny<string>(), It.IsAny<string>(),
                null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserRepresentation());
        
        UsersApiMock.Setup(x => x.PutUsersByUserIdAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserRepresentation>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await TestCandidate.UpdateUserAsync(new User(), CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CommandResult>();
        result.Data.Should().BeNull();
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
