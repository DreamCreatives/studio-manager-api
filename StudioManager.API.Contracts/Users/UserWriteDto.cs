namespace StudioManager.API.Contracts.Users;

public sealed record UserWriteDto(
    string FirstName,
    string LastName,
    string Email);
