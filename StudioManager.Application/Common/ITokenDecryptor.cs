namespace StudioManager.Application.Common;

public interface ITokenDecryptor : IApplicationService
{
    public string? UserId { get; }
    public string? ClientId { get; }
}
