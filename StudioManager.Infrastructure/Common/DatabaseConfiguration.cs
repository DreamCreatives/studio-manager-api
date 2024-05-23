using System.Diagnostics.CodeAnalysis;

namespace StudioManager.Infrastructure.Common;

[ExcludeFromCodeCoverage]
public class DatabaseConfiguration
{
    public const string NodeName = "DatabaseConfiguration";

    public DatabaseConfigurationNode Write { get; set; } = null!;
}
