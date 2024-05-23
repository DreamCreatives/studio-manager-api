using FluentAssertions;
using NUnit.Framework;
using StudioManager.API.Contracts.Equipments;
using StudioManager.Application.Equipments.Create;
using StudioManager.Domain.Entities;
using StudioManager.Domain.ErrorMessages;
using StudioManager.Infrastructure;
using StudioManager.Tests.Common;
using StudioManager.Tests.Common.DbContextExtensions;

namespace StudioManager.Application.Tests.Equipments.CreateEquipmentCommandHandlerTests;

public class Handle : IntegrationTestBase
{
    private static CreateEquipmentCommandHandler _testCandidate = null!;
    private static TestDbContextFactory<StudioManagerDbContext> _testDbContextFactory = null!;

    [SetUp]
    public async Task SetUpAsync()
    {
        var connectionString = await DbMigrator.MigrateDbAsync();
        _testDbContextFactory = new TestDbContextFactory<StudioManagerDbContext>(connectionString);
        _testCandidate = new CreateEquipmentCommandHandler(_testDbContextFactory);
    }

    [Test]
    public async Task should_return_error_when_equipment_type_does_not_exist_async()
    {
        // Arrange
        var dto = new EquipmentWriteDto("Equipment-Test-Name", Guid.NewGuid(), 1);
        var command = new CreateEquipmentCommand(dto);

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
    public async Task should_return_error_when_equipment_has_duplicated_name_and_type_async()
    {
        // Arrange

        var equipmentType = EquipmentType.Create("Test-Equipment-Type");
        var existing = Equipment.Create("Equipment-Test-Name", equipmentType.Id, 1);
        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await AddEntitiesToTable(dbContext, equipmentType);
            await AddEntitiesToTable(dbContext, existing);
        }

        var dto = new EquipmentWriteDto("Equipment-Test-Name", existing.EquipmentTypeId, 1);
        var command = new CreateEquipmentCommand(dto);

        // Act
        var result = await _testCandidate.Handle(command, Cts.Token);

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(ConflictStatusCode);
        result.Data.Should().BeNull();
        result.Error.Should().NotBeNullOrWhiteSpace();
        result.Error.Should().Be(string.Format(DB_FORMAT.EQUIPMENT_DUPLICATE_NAME_TYPE,
            existing.Name, existing.EquipmentTypeId));
    }

    [Test]
    public async Task should_return_success_async()
    {
        // Arrange

        var equipmentType = EquipmentType.Create("Test-Equipment-Type");
        await using (var dbContext = await _testDbContextFactory.CreateDbContextAsync(Cts.Token))
        {
            await ClearTableContentsForAsync<EquipmentType>(dbContext);
            await ClearTableContentsForAsync<Equipment>(dbContext);
            await AddEntitiesToTable(dbContext, equipmentType);
        }

        var dto = new EquipmentWriteDto("Equipment-Test-Name", equipmentType.Id, 1);
        var command = new CreateEquipmentCommand(dto);

        // Act
        var result = await _testCandidate.Handle(command, Cts.Token);

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(OkStatusCode);
        result.Data.Should().NotBeNull();
        result.Error.Should().BeNullOrWhiteSpace();
        var parseResult = Guid.TryParse(result.Data!.ToString(), out var guid);
        parseResult.Should().BeTrue();
        guid.Should().NotBeEmpty();
    }
}
