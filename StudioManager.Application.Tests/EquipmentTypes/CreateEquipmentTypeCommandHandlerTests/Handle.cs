using System.Net;
using FluentAssertions;
using NUnit.Framework;
using StudioManager.API.Contracts.EquipmentTypes;
using StudioManager.Application.EquipmentTypes.Create;
using StudioManager.Application.Tests.EquipmentTypes.Common;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Entities;
using StudioManager.Domain.ErrorMessages;
using StudioManager.Infrastructure;
using StudioManager.Tests.Common;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Application.Tests.EquipmentTypes.CreateEquipmentTypeCommandHandlerTests;

public sealed class Handle : IntegrationTestBase
{
    private static CreateEquipmentTypeCommandHandler _testCandidate = null!;
    private static TestDbContextFactory<StudioManagerDbContext> _testDbContextFactory = null!;

    [SetUp]
    public async Task SetUpAsync()
    {
        var connectionString = await DbMigrator.MigrateDbAsync();
        _testDbContextFactory = new TestDbContextFactory<StudioManagerDbContext>(connectionString);
        _testCandidate = new CreateEquipmentTypeCommandHandler(_testDbContextFactory);
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

        var command = new CreateEquipmentTypeCommand(new EquipmentTypeWriteDto(equipmentType.Name));
        
        // Act
        var result = await _testCandidate.Handle(command, Cts.Token);

        result.Should().NotBeNull();
        result.Should().BeOfType<CommandResult>();
        result.Data.Should().BeNull();
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.Conflict);
        result.Error.Should().NotBeNullOrWhiteSpace();
        result.Error.Should().Be(DB.EQUIPMENT_TYPE_DUPLICATE_NAME);
    }
    
    [Test]
    public async Task should_return_success_async()
    {
        // Arrange
        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await EquipmentTypeTestContextHelper.ClearEquipmentTypesAsync(dbContext);
        }
        
        var equipmentType = EquipmentType.Create("Test-Equipment-Type");

        var command = new CreateEquipmentTypeCommand(new EquipmentTypeWriteDto(equipmentType.Name));
        
        // Act
        var result = await _testCandidate.Handle(command, Cts.Token);

        result.Should().NotBeNull();
        result.Should().BeOfType<CommandResult>();
        result.Data.Should().NotBeNull();
        var parseResult = Guid.TryParse(result.Data!.ToString(), out var id);
        parseResult.Should().BeTrue();
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Error.Should().BeNullOrWhiteSpace();
        
        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            var databaseCheck = await dbContext.EquipmentTypes.FindAsync(id);
            databaseCheck.Should().NotBeNull();
            databaseCheck!.Name.Should().Be(equipmentType.Name);
        }
    }
}