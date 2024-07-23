namespace StudioManager.API.Contracts.Users;

public sealed record UserReadDto(Guid Id, string KeycloakId, string FirstName, string LastName, string Email);
