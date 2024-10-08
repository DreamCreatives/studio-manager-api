﻿using FS.Keycloak.RestApiClient.Model;
using StudioManager.Domain.Entities;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.Application.KeyCloak;

public interface IKeyCloakService
{
    Task<CommandResult> AddUserAsync(User user, CancellationToken cancellationToken = default);
    Task<CommandResult> UpdateUserAsync(User user, CancellationToken cancellationToken = default);
    Task<UserRepresentation?> GetIdentityUserByEmail(string email, CancellationToken cancellationToken = default);
    Task<CommandResult> RemoveUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<UserRepresentation?> GetIdentityUserById(string keycloakId, CancellationToken cancellationToken = default);
}
