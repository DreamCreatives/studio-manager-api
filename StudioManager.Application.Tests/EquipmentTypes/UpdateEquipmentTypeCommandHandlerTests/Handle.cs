using FluentAssertions;
using NUnit.Framework;
using StudioManager.API.Contracts.EquipmentTypes;
using StudioManager.Application.EquipmentTypes.Update;
using StudioManager.Application.Tests.EquipmentTypes.Common;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Entities;
using StudioManager.Domain.ErrorMessages;
using StudioManager.Infrastructure;
using StudioManager.Tests.Common;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Application.Tests.EquipmentTypes.UpdateEquipmentTypeCommandHandlerTests;

public sealed class Handle : IntegrationTestBase
{
    private static UpdateEquipmentTypeCommandHandler _testCandidate = null!;
    private static TestDbContextFactory<StudioManagerDbContext> _testDbContextFactory = null!;

    [SetUp]
    public async Task SetUpAsync()
    {
        var connectionString = await DbMigrator.MigrateDbAsync();
        _testDbContextFactory = new TestDbContextFactory<StudioManagerDbContext>(connectionString);
        _testCandidate = new UpdateEquipmentTypeCommandHandler(_testDbContextFactory);
    }
    
    [Test]
    public async Task should_return_conflict_when_the_name_is_duplicated_async()
    {
        // Arrange
        var equipmentType = EquipmentType.Create("Test-Equipment-Type");
        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await EquipmentTypeTestContextHelper.AddEquipmentTypeAsync(dbContext, equipmentType);
        }

        var command = new UpdateEquipmentTypeCommand(Guid.NewGuid(), new EquipmentTypeWriteDto(equipmentType.Name));
        
        // Act
        var result = await _testCandidate.Handle(command, Cts.Token);

        result.Should().NotBeNull();
        result.Should().BeOfType<CommandResult>();
        result.Data.Should().BeNull();
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(ConflictStatusCode);
        result.Error.Should().NotBeNullOrWhiteSpace();
        result.Error.Should().Be(DB.EQUIPMENT_TYPE_DUPLICATE_NAME);
    }
    
    [Test]
    public async Task should_return_not_found_when_updating_non_existing_entity_async()
    {
        // Arrange
        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await EquipmentTypeTestContextHelper.ClearEquipmentTypesAsync(dbContext);
        }
        
        var equipmentType = EquipmentType.Create("Test-Equipment-Type");
        var command = new UpdateEquipmentTypeCommand(equipmentType.Id, new EquipmentTypeWriteDto(equipmentType.Name));
        
        // Act
        var result = await _testCandidate.Handle(command, Cts.Token);

        result.Should().NotBeNull();
        result.Should().BeOfType<CommandResult>();
        result.Data.Should().BeNull();
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(NotFoundStatusCode);
        result.Error.Should().NotBeNullOrWhiteSpace();
        result.Error.Should().Be($"[NOT FOUND] {equipmentType.GetType().Name} with id '{equipmentType.Id}' does not exist");
    }
    
    [Test]
    public async Task should_return_success_async()
    {
        // Arrange
        var equipmentType = EquipmentType.Create("Test-Equipment-Type");
        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await EquipmentTypeTestContextHelper.AddEquipmentTypeAsync(dbContext, equipmentType);
        }
        equipmentType.Update("Updated-Equipment-Type");
        var command = new UpdateEquipmentTypeCommand(equipmentType.Id, new EquipmentTypeWriteDto(equipmentType.Name));
        
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
            databaseCheck.Should().NotBeNull();
            databaseCheck!.Name.Should().Be(equipmentType.Name);
        }
    }
}