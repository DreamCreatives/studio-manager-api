using FluentAssertions;
using NUnit.Framework;
using StudioManager.API.Contracts.Users;
using StudioManager.Application.Users.GetById;
using StudioManager.Domain.Entities;
using StudioManager.Infrastructure;
using StudioManager.Infrastructure.Common.Results;
using StudioManager.Tests.Common;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Application.Tests.Users.GetUserByIdQueryHandlerTests;

public sealed class Handle : IntegrationTestBase
{
    private static GetUserByIdQueryHandler _testCandidate = null!;
    private static TestDbContextFactory<StudioManagerDbContext> _dbContextFactory = null!;

    [SetUp]
    public async Task SetUpAsync()
    {
        var connectionString = await DbMigrator.MigrateDbAsync();
        _dbContextFactory = new TestDbContextFactory<StudioManagerDbContext>(connectionString);
        
        _testCandidate = new GetUserByIdQueryHandler(_dbContextFactory, Mapper);
    }

    [Test]
    public async Task When_UserIsNotFoundInDatabase_Should_Return_FailedQueryResult()
    {
        // Arrange
        var query = new GetUserByIdQuery(Guid.NewGuid());
        await using (var dbContext = await _dbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await ClearTableContentsForAsync<User>(dbContext);
        }
        
        // Act
        var result = await _testCandidate.Handle(query, Cts.Token);
        
        // Assert
        result.Should().BeEquivalentTo(QueryResult<UserReadDto>.NotFound(query.Id));
    }
    
    [Test]
    public async Task When_UserIsFoundInDatabase_Should_Return_MappedUserWithSuccessResult()
    {
        // Arrange
        var user = User.Create(Faker.Name.First(), Faker.Name.Last(), Faker.Internet.Email());
        await using (var dbContext = await _dbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await ClearTableContentsForAsync<User>(dbContext);

            await dbContext.Users.AddAsync(user, Cts.Token);
            await dbContext.SaveChangesAsync(Cts.Token);
        }
        
        // Act
        var result = await _testCandidate.Handle(new GetUserByIdQuery(user.Id), Cts.Token);
        
        // Assert
        var dto = new UserReadDto(user.Id, user.KeycloakId, user.FirstName, user.LastName, user.Email);
        result.Should().BeEquivalentTo(QueryResult.Success(dto));
    }
}
