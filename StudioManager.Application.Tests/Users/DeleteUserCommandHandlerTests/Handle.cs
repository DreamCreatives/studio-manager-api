using FluentAssertions;
using NUnit.Framework;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Application.KeyCloak;
using StudioManager.Application.Users.Delete;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Entities;
using StudioManager.Infrastructure;
using StudioManager.Tests.Common;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Application.Tests.Users.DeleteUserCommandHandlerTests;

public sealed class Handle : IntegrationTestBase
{
    private static readonly User TestUser = User.Create("Test", "User", "corp.szczur@corp.euro.kolhoz.eu");
    private static readonly Mock<IKeyCloakService> KeyCloakServiceMock = new();
    private static readonly DeleteUserCommand Command = new(TestUser.Id);
    
    private static DeleteUserCommandHandler _testCandidate = null!;
    private static TestDbContextFactory<StudioManagerDbContext> _dbContextFactory = null!;

    [SetUp]
    public async Task SetUpAsync()
    {
        var connectionString = await DbMigrator.MigrateDbAsync();
        _dbContextFactory = new TestDbContextFactory<StudioManagerDbContext>(connectionString);
        
        _testCandidate = new DeleteUserCommandHandler(_dbContextFactory, KeyCloakServiceMock.Object);
    }

    [Test]
    public async Task When_UserIsNotFoundInDatabase_ShouldReturn_NotFound()
    {
        // Arrange
        await using (var dbContext = await _dbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await ClearTableContentsForAsync<User>(dbContext, x => x.Id.Equals(TestUser.Id));
        }
        
        // Act
        var result = await _testCandidate.Handle(Command, Cts.Token);
        
        // Assert
        result.Should().BeEquivalentTo(CommandResult.NotFound<User>(Command.Id));
        KeyCloakServiceMock.Verify(x => x.RemoveUserAsync(It.IsNotIn<string>("test-keycloak-id", "test-keycloak-id1"),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task When_UserIsFoundInDatabase_But_KeyCloakService_Returns_FailedResult_Should_Return_KeyCloakResult()
    {
        // Arrange
        TestUser.SetNewIdentityId("test-keycloak-id");
        await using (var dbContext = await _dbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await AddUserToDatabaseAsync(dbContext);
        }
        
        var keyCloakResult = CommandResult.UnexpectedError("KeyCloakError");
        
        KeyCloakServiceMock.Setup(x => x.RemoveUserAsync(It.Is<string>(y => y.Equals(TestUser.KeycloakId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(keyCloakResult);
        
        // Act
        var result = await _testCandidate.Handle(Command, Cts.Token);
        
        // Assert
        result.Should().BeEquivalentTo(keyCloakResult);
        KeyCloakServiceMock.Verify(x => x.RemoveUserAsync(It.Is<string>(y => y.Equals(TestUser.KeycloakId)), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task When_UserIsFoundInDatabase_AndRemovedFromKeyCloak_ShouldReturn_Success()
    {
        // Arrange
        TestUser.SetNewIdentityId("test-keycloak-id1");
        await using (var dbContext = await _dbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await AddUserToDatabaseAsync(dbContext);
        }
        
        KeyCloakServiceMock.Setup(x => x.RemoveUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CommandResult.Success());
        
        // Act
        var result = await _testCandidate.Handle(Command, Cts.Token);
        
        // Assert
        result.Should().BeEquivalentTo(CommandResult.Success());
        await using (var dbContext = await _dbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            var user = await dbContext.GetUserByIdAsync(Command.Id, Cts.Token);
            user.Should().BeNull();
        }
        
        KeyCloakServiceMock.Verify(x => x.RemoveUserAsync(It.Is<string>(y => y.Equals(TestUser.KeycloakId)), It.IsAny<CancellationToken>()), Times.Once);
    }

    private static async Task AddUserToDatabaseAsync(StudioManagerDbContext dbContext)
    {
        await ClearTableContentsForAsync<User>(dbContext, x => x.Id.Equals(TestUser.Id));
        
        await dbContext.Users.AddAsync(TestUser, Cts.Token);
        await dbContext.SaveChangesAsync(Cts.Token);
    }
}
