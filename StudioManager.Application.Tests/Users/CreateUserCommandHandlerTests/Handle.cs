using FluentAssertions;
using FS.Keycloak.RestApiClient.Model;
using NUnit.Framework;
using StudioManager.API.Contracts.Users;
using StudioManager.Application.KeyCloak;
using StudioManager.Application.Users.Create;
using StudioManager.Domain.Entities;
using StudioManager.Infrastructure;
using StudioManager.Infrastructure.Common.Results;
using StudioManager.Tests.Common;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Application.Tests.Users.CreateUserCommandHandlerTests;

public sealed class Handle : IntegrationTestBase
{
    private static readonly CreateUserCommand Command = new(
        new UserWriteDto("John", "Doe", "john.doe@corp.com"));
    private static TestDbContextFactory<StudioManagerDbContext> _dbContextFactory = null!;
    private static CreateUserCommandHandler _testCandidate = null!;
    private static Mock<IKeyCloakService> _keyCloakServiceMock = null!;

    [SetUp]
    public async Task SetUpAsync()
    {
        _keyCloakServiceMock = new Mock<IKeyCloakService>();
        var connectionString = await DbMigrator.MigrateDbAsync();
        _dbContextFactory = new TestDbContextFactory<StudioManagerDbContext>(connectionString);
        _testCandidate = new CreateUserCommandHandler(_dbContextFactory, _keyCloakServiceMock.Object);
    }

    [Test]
    public async Task When_UserWithDuplicateEmailExists_Should_Return_Conflict()
    {
        // Arrange
        await using (var dbContext = await _dbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await ClearTableContentsForAsync<User>(dbContext, x => x.Email.Equals(Command.User.Email));
            var user = User.Create(Command.User.FirstName, Command.User.LastName, Command.User.Email);
        
            await dbContext.Users.AddAsync(user, Cts.Token);
            await dbContext.SaveChangesAsync(Cts.Token);
        }
        
        // Act
        var result = await _testCandidate.Handle(Command, Cts.Token);
        
        // Assert
        result.Should().BeEquivalentTo(CommandResult.Conflict($"User with email {Command.User.Email} already exists."));
        
        await using var dbContext1 = await _dbContextFactory.CreateDbContextAsync(Cts.Token);
        await ClearTableContentsForAsync<User>(dbContext1);
    }

    [Test]
    public async Task When_KeyCloakService_AddUserAsync_Returns_Failure_Then_Returns_Failure()
    {
        // Arrange
        var keycloakResult = CommandResult.UnexpectedError("test-error");
        _keyCloakServiceMock.Setup(x => x.AddUserAsync(
                It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(keycloakResult);
        
        // Act
        var result = await _testCandidate.Handle(Command, Cts.Token);
        
        // Assert
        result.Should().BeEquivalentTo(keycloakResult);
    }
    
    [Test]
    public async Task When_KeyCloakService_GetIdentityUserByEmail_Returns_Null_Then_Returns_NotFound()
    {
        // Arrange
        _keyCloakServiceMock.Setup(x => x.AddUserAsync(
                It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CommandResult.Success());
        _keyCloakServiceMock.Setup(x => x.GetIdentityUserByEmail(
                It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserRepresentation?)null);
        
        // Act
        var result = await _testCandidate.Handle(Command, Cts.Token);
        
        // Assert
        result.Should().BeEquivalentTo(CommandResult.NotFound<UserRepresentation>());
    }
    
    [Test]
    public async Task When_UserIsAddedToKeyCloak_And_IdentityUserIsFound_Then_UserIsAddedToDatabase()
    {
        // Arrange
        var identityUser = new UserRepresentation { Id = Guid.NewGuid().ToString() };
        _keyCloakServiceMock.Setup(x => x.AddUserAsync(
                It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CommandResult.Success());
        _keyCloakServiceMock.Setup(x => x.GetIdentityUserByEmail(
                It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(identityUser);
        
        // Act
        var result = await _testCandidate.Handle(Command, Cts.Token);
        
        // Assert
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(Cts.Token);
        var user = await dbContext.Users.FindAsync(result.Data);
        user.Should().NotBeNull();
        user!.KeycloakId.Should().Be(identityUser.Id);
        result.Should().BeEquivalentTo(CommandResult.Success(user.Id));
    }
}
