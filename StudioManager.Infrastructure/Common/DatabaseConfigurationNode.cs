namespace StudioManager.Infrastructure.Common;

public sealed class DatabaseConfigurationNode(
    string password, string userName, string connectionDetails)
{
    private string Password { get; } = password;
    private string Username { get; } = userName;
    private string ConnectionDetails { get; } = connectionDetails;

    public string GetConnectionString()
    {
        return $"{ConnectionDetails}User Id = {Username};Password = {Password};";
    }
}