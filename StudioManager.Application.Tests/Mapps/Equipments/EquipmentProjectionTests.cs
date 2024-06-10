using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;
using StudioManager.API.Contracts.Equipments;
using StudioManager.API.Contracts.EquipmentTypes;
using StudioManager.Domain.Entities;
using StudioManager.Tests.Common.AutoMapperExtensions;

namespace StudioManager.Application.Tests.Mapps.Equipments;

[ExcludeFromCodeCoverage]
public sealed class EquipmentProjectionTests
{
    [Test]
    public void should_map_equipment_to_equipment_projection()
    {
        // Arrange
        var equipmentType = EquipmentType.Create("Test Equipment Type");
        var equipment = Equipment.Create("Test Equipment", equipmentType.Id, 1);
        equipment.EquipmentType = equipmentType;
        var mapper = MappingTestHelper.Mapper;

        // Act
        var result = mapper.Map<EquipmentReadDto>(equipment);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Id.Should().Be(equipment.Id);
        result.Name.Should().NotBeNullOrWhiteSpace();
        result.Name.Should().Be(equipment.Name);
        result.InitialQuantity.Should().Be(equipment.InitialQuantity);
        result.EquipmentType.Should().NotBeNull();
        result.EquipmentType.Should().BeOfType<EquipmentTypeReadDto>();
        result.EquipmentType.Id.Should().Be(equipmentType.Id);
        result.EquipmentType.Name.Should().NotBeNullOrWhiteSpace();
        result.EquipmentType.Name.Should().Be(equipmentType.Name);
    }
}
