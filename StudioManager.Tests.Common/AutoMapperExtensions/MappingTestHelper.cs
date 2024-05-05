using System.Diagnostics.CodeAnalysis;
using AutoMapper;

namespace StudioManager.Tests.Common.AutoMapperExtensions;

[ExcludeFromCodeCoverage]
public static class MappingTestHelper
{
    private static IMapper? _mapper;
    private static MapperConfiguration? _mapperConfiguration;
    public static MapperConfiguration MapperConfiguration => GetMapperConfiguration();
    public static IMapper Mapper => GetMapper();
    
    private static MapperConfiguration GetMapperConfiguration()
    {
        return _mapperConfiguration ??= new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(Application.DependencyInjection).Assembly);
        });
    }
    
    private static IMapper GetMapper()
    {
        return _mapper ??= MapperConfiguration.CreateMapper();
    }
}