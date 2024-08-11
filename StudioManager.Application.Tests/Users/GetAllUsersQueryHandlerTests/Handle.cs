using FluentAssertions;
using NUnit.Framework;
using StudioManager.API.Contracts.Pagination;
using StudioManager.API.Contracts.Users;
using StudioManager.Application.Users.GetAll;
using StudioManager.Domain.Entities;
using StudioManager.Domain.Filters.Builders;
using StudioManager.Infrastructure;
using StudioManager.Infrastructure.Common.Results;
using StudioManager.Tests.Common;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Application.Tests.Users.GetAllUsersQueryHandlerTests;

public sealed class Handle : IntegrationTestBase
{
    private static readonly GetAllUsersQuery Query = new(UserFilterBuilder.New().Build(), new PaginationDto());
    
    private static GetAllUsersQueryHandler _testCandidate = null!;
    private static TestDbContextFactory<StudioManagerDbContext> _dbContextFactory = null!;

    [SetUp]
    public async Task SetUpAsync()
    {
        var connectionString = await DbMigrator.MigrateDbAsync();
        _dbContextFactory = new TestDbContextFactory<StudioManagerDbContext>(connectionString);
        _testCandidate = new GetAllUsersQueryHandler(_dbContextFactory, Mapper);
    }
    
    [Test]
    public async Task WhenCalled_ShouldReturnAllUsers()
    {
        // Arrange
        var expectedResult = QueryResult<PagingResultDto<UserReadDto>>.Success(
            new PagingResultDto<UserReadDto>
            {
                Pagination = new PaginationDetailsDto
                {
                    Page = PaginationDto.DefaultPage,
                    Limit = PaginationDto.DefaultLimit,
                    Total = 0
                },
                Data = []
            });

        await using (var dbContext = await _dbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await ClearTableContentsForAsync<User>(dbContext);
        }
        
        // Act
        var result = await _testCandidate.Handle(Query, Cts.Token);
        
        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
}
