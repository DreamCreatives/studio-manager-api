namespace StudioManager.API.Contracts.Common;

public class NamedBaseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
}