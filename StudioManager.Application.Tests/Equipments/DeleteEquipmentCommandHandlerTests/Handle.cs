using FluentAssertions;
using NUnit.Framework;
using StudioManager.Application.Equipments.Delete;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Entities;
using StudioManager.Infrastructure;
using StudioManager.Tests.Common;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Application.Tests.Equipments.DeleteEquipmentCommandHandlerTests;

public sealed class Handle : IntegrationTestBase
{
    private static DeleteEquipmentCommandHandler _testCandidate = null!;
    private static TestDbContextFactory<StudioManagerDbContext> _testDbContextFactory = null!;


    [SetUp]
    public async Task SetUpAsync()
    {
        var connectionString = await DbMigrator.MigrateDbAsync();
        _testDbContextFactory = new TestDbContextFactory<StudioManagerDbContext>(connectionString);
        _testCandidate = new DeleteEquipmentCommandHandler(_testDbContextFactory);
    }

    [Test]
    public async Task should_return_not_found_when_removing_non_existing_entity_async()
    {
        // Arrange
        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await ClearTableContentsForAsync<Equipment>(dbContext);
        }

        var id = Guid.NewGuid();

        var command = new DeleteEquipmentCommand(id);

        // Act
        var result = await _testCandidate.Handle(command, Cts.Token);

        result.Should().NotBeNull();
        result.Should().BeOfType<CommandResult>();
        result.Data.Should().BeNull();
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(NotFoundStatusCode);
        result.Error.Should().NotBeNullOrWhiteSpace();
        result.Error.Should().Be($"[NOT FOUND] {nameof(Equipment)} with id '{id}' does not exist");
    }

    [Test]
    public async Task should_return_success_async()
    {
        // Arrange
        var equipmentType = EquipmentType.Create("Test-Equipment-Type");
        var equipment = Equipment.Create("Test-Equipment", equipmentType.Id, 1);
        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await ClearTableContentsForAsync<EquipmentType>(dbContext);
            await ClearTableContentsForAsync<Equipment>(dbContext);
            await AddEntitiesToTable(dbContext, equipmentType);
            await AddEntitiesToTable(dbContext, equipment);
        }

        var command = new DeleteEquipmentCommand(equipment.Id);

        // Act
        var result = await _testCandidate.Handle(command, Cts.Token);

        result.Should().NotBeNull();
        result.Should().BeOfType<CommandResult>();
        result.Data.Should().BeNull();
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(OkStatusCode);
        result.Error.Should().BeNullOrWhiteSpace();

        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            var databaseCheck = await dbContext.Equipments.FindAsync(equipment.Id);
            databaseCheck.Should().BeNull();
        }
    }
}
