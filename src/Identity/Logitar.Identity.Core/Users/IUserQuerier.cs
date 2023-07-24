﻿using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Core.Users;

public interface IUserQuerier
{
  Task<User> ReadAsync(UserAggregate user, CancellationToken cancellationToken = default);
  Task<User?> ReadAsync(string id, CancellationToken cancellationToken = default);
  Task<User?> ReadAsync(string? tenantId, string uniqueName, CancellationToken cancellationToken = default);
  Task<IEnumerable<User>> ReadAsync(string? tenantId, IEmailAddress email, CancellationToken cancellationToken = default);
}
