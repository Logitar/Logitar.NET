using Logitar.Identity.Core.Users.Models;
using MediatR;

namespace Logitar.Identity.Core.Users.Queries;

public record ReadUserQuery : IRequest<User?>
{
  public ReadUserQuery(string? id = null, string? tenantId = null, string? uniqueName = null,
    string? externalIdentifierKey = null, string? externalIdentifierValue = null)
  {
    Id = id;
    TenantId = tenantId;
    UniqueName = uniqueName;
    ExternalIdentifierKey = externalIdentifierKey;
    ExternalIdentifierValue = externalIdentifierValue;
  }

  public string? Id { get; }
  public string? TenantId { get; }
  public string? UniqueName { get; }
  public string? ExternalIdentifierKey { get; }
  public string? ExternalIdentifierValue { get; }
}
