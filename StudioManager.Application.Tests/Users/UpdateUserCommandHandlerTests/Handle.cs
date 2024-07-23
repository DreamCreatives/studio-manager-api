using FluentAssertions;
using NUnit.Framework;
using StudioManager.API.Contracts.Users;
using StudioManager.Application.DbContextExtensions;
using StudioManager.Application.KeyCloak;
using StudioManager.Application.Users.Update;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Entities;
using StudioManager.Infrastructure;
using StudioManager.Tests.Common;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Application.Tests.Users.UpdateUserCommandHandlerTests;

public sealed class Handle : IntegrationTestBase
{
    private static readonly User TestUser = User.Create(Faker.Name.First(), Faker.Name.Last(), Faker.Internet.Email());
    private static readonly UpdateUserCommand Command = new(TestUser.Id,
        new UserWriteDto(Faker.Name.First(), Faker.Name.Last(), Faker.Internet.Email()));
    private static readonly Mock<IKeyCloakService> KeyCloakServiceMock = new();
    
    private static UpdateUserCommandHandler _testCandidate = null!;
    private static TestDbContextFactory<StudioManagerDbContext> _testDbContextFactory = null!;

    [SetUp]
    public async Task SetUpAsync()
    {
        var connectionString = await DbMigrator.MigrateDbAsync();
        _testDbContextFactory = new TestDbContextFactory<StudioManagerDbContext>(connectionString);
        
        _testCandidate = new UpdateUserCommandHandler(_testDbContextFactory, KeyCloakServiceMock.Object);
    }

    [Test]
    public async Task When_UserWithTheSameEmailExists_ShouldReturn_ConflictCommandResult()
    {
        // Arrange
        TestUser.SetNewIdentityId(Faker.Identification.UsPassportNumber());
        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await ClearTableContentsForAsync<User>(dbContext);

            await dbContext.Users.AddAsync(TestUser, Cts.Token);
            await dbContext.SaveChangesAsync(Cts.Token);
        }
        
        // Act
        var result = await _testCandidate.Handle(new UpdateUserCommand(Guid.NewGuid(), Command.User with { Email = TestUser.Email }), Cts.Token);
        
        // Assert
        result.Should().BeEquivalentTo(CommandResult.Conflict($"User with email {TestUser.Email} already exists"));
        KeyCloakServiceMock.Verify(x => x.UpdateUserAsync(It.Is<User>(y => y.KeycloakId == TestUser.KeycloakId), Cts.Token), Times.Never);
    }
    
    [Test]
    public async Task When_UserWithTheSameEmailDoesNotExist_ButUserDoesNotExist_ShouldReturn_NotFoundCommandResult()
    {
        // Arrange
        TestUser.SetNewIdentityId(Faker.Identification.BulgarianPin());
        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await ClearTableContentsForAsync<User>(dbContext);
        }
        
        // Act
        var result = await _testCandidate.Handle(Command, Cts.Token);
        
        // Assert
        result.Should().BeEquivalentTo(CommandResult.NotFound<User>(Command.Id));
        KeyCloakServiceMock.Verify(x => x.UpdateUserAsync(It.Is<User>(y => y.KeycloakId == TestUser.KeycloakId), Cts.Token), Times.Never);
    }
    
    [Test]
    public async Task When_UserWithTheSameEmailDoesNotExist_And_UserDoesExist_ButKeyCloakReturnsFail_Should_ReturnKeyCloakResult()
    {        
        // Arrange
        TestUser.SetNewIdentityId(Faker.Identification.UsPassportNumber());
        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await ClearTableContentsForAsync<User>(dbContext);

            await dbContext.Users.AddAsync(TestUser, Cts.Token);
            await dbContext.SaveChangesAsync(Cts.Token);
        }
        
        KeyCloakServiceMock.Setup(x => x.UpdateUserAsync(It.IsAny<User>(), Cts.Token))
            .ReturnsAsync(CommandResult.UnexpectedError("KeyCloak error"));
        
        // Act
        var result = await _testCandidate.Handle(Command, Cts.Token);
        
        // Assert
        result.Should().BeEquivalentTo(CommandResult.UnexpectedError("KeyCloak error"));
        KeyCloakServiceMock.Verify(x => x.UpdateUserAsync(It.Is<User>(y => y.KeycloakId == TestUser.KeycloakId), Cts.Token), Times.Once);
    }
    
    [Test]
    public async Task When_UserWithTheSameEmailDoesNotExist_And_UserDoesExist_And_KeyCloakReturnsSuccess_ShouldUpdateUser()
    {        
        // Arrange
        TestUser.SetNewIdentityId(Faker.Identification.UsPassportNumber());
        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await ClearTableContentsForAsync<User>(dbContext);

            await dbContext.Users.AddAsync(TestUser, Cts.Token);
            await dbContext.SaveChangesAsync(Cts.Token);
        }
        
        KeyCloakServiceMock.Setup(x => x.UpdateUserAsync(It.IsAny<User>(), Cts.Token))
            .ReturnsAsync(CommandResult.Success());
        
        // Act
        var result = await _testCandidate.Handle(Command, Cts.Token);
        
        // Assert
        result.Should().BeEquivalentTo(CommandResult.Success());
        KeyCloakServiceMock.Verify(x => x.UpdateUserAsync(It.Is<User>(y => y.KeycloakId == TestUser.KeycloakId), Cts.Token), Times.Once);
        
        await using var dbContext1 = await _testDbContextFactory.CreateDbContextAsync(Cts.Token);
        var updatedUser = await dbContext1.GetUserByIdAsync(TestUser.Id);
        updatedUser.Should().NotBeNull();
        updatedUser!.FirstName.Should().Be(Command.User.FirstName);
        updatedUser.LastName.Should().Be(Command.User.LastName);
        updatedUser.Email.Should().Be(Command.User.Email);
    }
}
