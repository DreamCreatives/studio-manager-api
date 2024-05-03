using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using StudioManager.API.Contracts.EquipmentTypes;
using StudioManager.Domain.Entities;
using StudioManager.Tests.Common.AutoMapperExtensions;
using Xunit;

namespace StudioManager.Application.Tests.Mapps.EquipmentTypes;

[ExcludeFromCodeCoverage]
public sealed class EquipmentTypeProjectionTests
{
    [Fact]
    public void should_map_equipment_type_to_equipment_type_projection()
    {
        // Arrange
        var equipmentType = EquipmentType.Create("Test Equipment Type");
        var mapper = MappingTestHelper.Mapper;
        
        // Act
        var result = mapper.Map<EquipmentTypeReadDto>(equipmentType);
        
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Id.Should().Be(equipmentType.Id);
        result.Name.Should().NotBeNullOrWhiteSpace();
        result.Name.Should().Be(equipmentType.Name);
    }
}