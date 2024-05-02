namespace SolutionManager.Infrastructure.Common;

public class DatabaseConfiguration
{
    public const string NodeName = "DatabaseConfiguration";
    
    public DatabaseConfigurationNode Write { get; set; } = null!;
    public DatabaseConfigurationNode Read { get; set; } = null!;
}