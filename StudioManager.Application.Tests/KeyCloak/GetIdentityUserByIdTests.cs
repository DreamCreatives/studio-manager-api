using FluentAssertions;
using FS.Keycloak.RestApiClient.Model;
using NUnit.Framework;

namespace StudioManager.Application.Tests.KeyCloak;

public sealed class GetIdentityUserByIdTests : KeyCloakServiceTestBase
{
    [Test]
    public async Task Should_return_null_on_api_exception()
    {
        // Arrange
        UsersApiMock.Setup(x => x.GetUsersByUserIdAsync(
                It.IsAny<string>(), It.IsAny<string>(),
                null, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await TestCandidate.GetIdentityUserByEmail(string.Empty);
        
        // Assert
        result.Should().BeNull();
    }

    [Test]
    [Ignore("Required keycloak to work")]
    public async Task Should_return_user_representation()
    {
        // Arrange
        var user = new UserRepresentation
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "John",
            LastName = "Doe",
            Email = "test@test.com"
        };

        UsersApiMock.Setup(x => x.GetUsersByUserIdAsync(
                It.IsAny<string>(), It.IsAny<string>(),
                null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act

        var result = await TestCandidate.GetIdentityUserByEmail(user.Email);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user);
    }
}
