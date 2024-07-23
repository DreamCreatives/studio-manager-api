using System.Net;
using FluentAssertions;
using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.Client;
using FS.Keycloak.RestApiClient.Model;
using NUnit.Framework;
using StudioManager.Domain.Common.Results;

namespace StudioManager.Application.Tests.KeyCloak;

public sealed class RemoveUserAsyncTests : KeyCloakServiceTestBase
{
    [Test]
    [Ignore("Required keycloak to work")]
    public async Task RemoveUserAsync_WhenUserExists_ReturnsSuccess()
    {
        // Arrange
        const string userId = "existingUserId";
        var mockUsersApi = new Mock<IUsersApiAsync>();
        mockUsersApi.Setup(api => api.DeleteUsersByUserIdAsync(It.IsAny<string>(), userId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await TestCandidate.RemoveUserAsync(userId);

        // Assert
        result.Should().BeEquivalentTo(CommandResult.Success());
    }

    [Test]
    [Ignore("Required keycloak to work")]
    public async Task RemoveUserAsync_WhenUserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        const string userId = "nonExistingUserId";
        var mockUsersApi = new Mock<IUsersApiAsync>();
        mockUsersApi.Setup(api => api.DeleteUsersByUserIdAsync(It.IsAny<string>(), userId, It.IsAny<CancellationToken>()))
            .Throws(new ApiException { ErrorCode = (int)HttpStatusCode.NotFound });

        // Act
        var result = await TestCandidate.RemoveUserAsync(userId);

        // Assert
        result.Should().BeEquivalentTo(CommandResult.NotFound<UserRepresentation>(userId));
    }

    [Test]
    [Ignore("Required keycloak to work")]
    public async Task RemoveUserAsync_WhenApiThrowsConflictException_ReturnsConflict()
    {
        // Arrange
        var userId = "conflictingUserId";
        var mockUsersApi = new Mock<IUsersApiAsync>();
        mockUsersApi.Setup(api => api.DeleteUsersByUserIdAsync(It.IsAny<string>(), userId, It.IsAny<CancellationToken>()))
            .Throws(new ApiException { ErrorCode = (int)HttpStatusCode.Conflict });

        // Act
        var result = await TestCandidate.RemoveUserAsync(userId);

        // Assert
        result.Should().BeEquivalentTo(CommandResult.Conflict(null!));
    }

    [Test]
    [Ignore("Required keycloak to work")]
    public async Task RemoveUserAsync_WhenApiThrowsUnexpectedException_ReturnsUnexpectedError()
    {
        // Arrange
        var userId = "userIdWithUnexpectedError";
        var mockUsersApi = new Mock<IUsersApiAsync>();
        mockUsersApi.Setup(api => api.DeleteUsersByUserIdAsync(It.IsAny<string>(), userId, It.IsAny<CancellationToken>()))
            .Throws(new ApiException(500, "Unexpected error"));

        // Act
        var result = await TestCandidate.RemoveUserAsync(userId);

        // Assert
        result.Should().BeEquivalentTo(CommandResult.UnexpectedError("Unexpected error"));
    }
}
