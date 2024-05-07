using FluentAssertions;
using NUnit.Framework;
using StudioManager.API.Contracts.Equipments;
using StudioManager.Application.Equipments.Update;
using StudioManager.Domain.Entities;
using StudioManager.Domain.ErrorMessages;
using StudioManager.Infrastructure;
using StudioManager.Tests.Common;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Application.Tests.Equipments.UpdateEquipmentCommandHandlerTests;

public sealed class Handle : IntegrationTestBase
{
    private static UpdateEquipmentCommandHandler _testCandidate = null!;
    private static TestDbContextFactory<StudioManagerDbContext> _testDbContextFactory = null!;

    [SetUp]
    public async Task SetUpAsync()
    {
        var connectionString = await DbMigrator.MigrateDbAsync();
        _testDbContextFactory = new TestDbContextFactory<StudioManagerDbContext>(connectionString);
        _testCandidate = new UpdateEquipmentCommandHandler(_testDbContextFactory);
    }

    [Test]
    public async Task should_return_not_found_when_equipment_type_does_not_exist_async()
    {
        // Arrange
        var dto = new EquipmentWriteDto("Test-Equipment", Guid.NewGuid(), 1);
        var command = new UpdateEquipmentCommand(Guid.NewGuid(), dto);
        
        // Act
        var result = await _testCandidate.Handle(command, Cts.Token);
        
        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(NotFoundStatusCode);
        result.Data.Should().BeNull();
        result.Error.Should().NotBeNullOrWhiteSpace();
        result.Error.Should().Be($"[NOT FOUND] {nameof(EquipmentType)} with id '{dto.EquipmentTypeId}' does not exist");
    }
    
    [Test]
    public async Task should_return_conflict_when_equipment_name_and_type_is_not_unique_async()
    {
        // Arrange
        var equipmentType = EquipmentType.Create("Test-Equipment-Type");
        var equipment = Equipment.Create("Test-Equipment", equipmentType.Id, 1);

        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await AddEntitiesToTable(dbContext, equipmentType);
            await AddEntitiesToTable(dbContext, equipment);
        }
        
        var dto = new EquipmentWriteDto(equipment.Name, equipmentType.Id, 100);
        var command = new UpdateEquipmentCommand(Guid.NewGuid(), dto);
        
        // Act
        var result = await _testCandidate.Handle(command, Cts.Token);
        
        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(ConflictStatusCode);
        result.Data.Should().BeNull();
        result.Error.Should().NotBeNullOrWhiteSpace();
        result.Error.Should().Be(string.Format(DB_FORMAT.EQUIPMENT_DUPLICATE_NAME_TYPE, 
            equipment.Name, equipmentType.Id));
    }
    
    [Test]
    public async Task should_return_not_found_when_equipment_does_not_exist_async()
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
        
        var dto = new EquipmentWriteDto("Test-Equipment-Updated", equipmentType.Id, 100);
        var command = new UpdateEquipmentCommand(Guid.NewGuid(), dto);
        
        // Act
        var result = await _testCandidate.Handle(command, Cts.Token);
        
        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(NotFoundStatusCode);
        result.Data.Should().BeNull();
        result.Error.Should().NotBeNullOrWhiteSpace();
        result.Error.Should().Be($"[NOT FOUND] {nameof(Equipment)} with id '{command.Id}' does not exist");
    }
    
    [Test]
    public async Task should_return_success_async()
    {
        // Arrange
        var equipmentType = EquipmentType.Create("Test-Equipment-Type");
        var equipment = Equipment.Create("Test-Equipment", equipmentType.Id, 1);

        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await AddEntitiesToTable(dbContext, equipmentType);
            await AddEntitiesToTable(dbContext, equipment);
        }
        
        var dto = new EquipmentWriteDto("Test-Equipment-Updated", equipmentType.Id, 100);
        var command = new UpdateEquipmentCommand(equipment.Id, dto);
        
        // Act
        var result = await _testCandidate.Handle(command, Cts.Token);
        
        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(OkStatusCode);
        result.Data.Should().BeNull();
        result.Error.Should().BeNullOrWhiteSpace();
    }
}