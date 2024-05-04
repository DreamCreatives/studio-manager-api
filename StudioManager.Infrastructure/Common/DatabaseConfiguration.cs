namespace StudioManager.Infrastructure.Common;

public class DatabaseConfiguration
{
    public const string NodeName = "DatabaseConfiguration";
    
    public DatabaseConfigurationNode Write { get; set; } = null!;
}