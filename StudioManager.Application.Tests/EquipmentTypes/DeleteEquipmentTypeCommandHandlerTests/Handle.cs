﻿using FluentAssertions;
using NUnit.Framework;
using StudioManager.Application.EquipmentTypes.Delete;
using StudioManager.Domain.Entities;
using StudioManager.Infrastructure;
using StudioManager.Infrastructure.Common.Results;
using StudioManager.Tests.Common;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Application.Tests.EquipmentTypes.DeleteEquipmentTypeCommandHandlerTests;

public sealed class Handle : IntegrationTestBase
{
    private static DeleteEquipmentTypeCommandHandler _testCandidate = null!;
    private static TestDbContextFactory<StudioManagerDbContext> _testDbContextFactory = null!;


    [SetUp]
    public async Task SetUpAsync()
    {
        var connectionString = await DbMigrator.MigrateDbAsync();
        _testDbContextFactory = new TestDbContextFactory<StudioManagerDbContext>(connectionString);
        _testCandidate = new DeleteEquipmentTypeCommandHandler(_testDbContextFactory);
    }

    [Test]
    public async Task should_return_not_found_when_removing_non_existing_entity_async()
    {
        // Arrange
        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await ClearTableContentsForAsync<EquipmentType>(dbContext);
        }

        var id = Guid.NewGuid();

        var command = new DeleteEquipmentTypeCommand(id);

        // Act
        var result = await _testCandidate.Handle(command, Cts.Token);

        result.Should().NotBeNull();
        result.Should().BeOfType<CommandResult>();
        result.Data.Should().BeNull();
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(NotFoundStatusCode);
        result.Error.Should().NotBeNullOrWhiteSpace();
        result.Error.Should().Be($"[NOT FOUND] {nameof(EquipmentType)} with id '{id}' does not exist");
    }

    [Test]
    public async Task should_return_success_async()
    {
        // Arrange
        var equipmentType = EquipmentType.Create("Test-Equipment-Type");
        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await AddEntitiesToTable(dbContext, equipmentType);
        }

        var command = new DeleteEquipmentTypeCommand(equipmentType.Id);

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
            var databaseCheck = await dbContext.EquipmentTypes.FindAsync(equipmentType.Id);
            databaseCheck.Should().BeNull();
        }
    }
}
