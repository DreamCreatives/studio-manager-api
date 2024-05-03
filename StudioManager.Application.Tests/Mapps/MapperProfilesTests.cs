using System.Diagnostics.CodeAnalysis;
using StudioManager.Tests.Common.AutoMapperExtensions;

namespace StudioManager.Application.Tests.Mapps;

[ExcludeFromCodeCoverage]
public sealed class MapperProfilesTests
{
    [Fact]
    public void mapper_configuration_is_valid()
    {
        var configuration = MappingTestHelper.MapperConfiguration;
        configuration.AssertConfigurationIsValid();
    }
}