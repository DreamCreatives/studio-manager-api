using System.Net;
using FluentAssertions;
using FS.Keycloak.RestApiClient.Model;
using NUnit.Framework;
using StudioManager.Domain.Entities;

namespace StudioManager.Application.Tests.KeyCloak;

public sealed class AddUserAsyncTests : KeyCloakServiceTestBase
{
    [Test]
    public async Task Should_return_Error_when_KeyCloak_throws_exception()
    {
        // Arrange
        UsersApiMock.Setup(x => x.PostUsersAsync(It.IsAny<string>(), It.IsAny<UserRepresentation>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Error"));

        var user = User.Create("John", "Doe", "john.doe@corp.com");
        
        // Act
        var result = await TestCandidate.AddUserAsync(user);
        
        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeFalse();
        result.Data.Should().BeNull();
        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Test]
    [Ignore("Required keycloak to work")]
    public async Task Should_return_Success_when_user_is_added()
    {
        // Arrange
        UsersApiMock.Setup(x => x.PostUsersAsync(It.IsAny<string>(), It.IsAny<UserRepresentation>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var user = User.Create("John", "Doe", "john.doe@corp.com");
        
        // Act
        var result = await TestCandidate.AddUserAsync(user);
        
        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeNull();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        
        UsersApiMock.Verify(x => x.PostUsersAsync(It.IsAny<string>(), It.IsAny<UserRepresentation>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
