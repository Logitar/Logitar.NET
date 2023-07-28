using Logitar.Identity.Core.Users.Models;
using MediatR;

namespace Logitar.Identity.Core.Users.Queries;

public record ReadUserQuery : IRequest<User?>
{
  public ReadUserQuery(string? id = null, string? tenantId = null, string? uniqueName = null)
  {
    Id = id;
    TenantId = tenantId;
    UniqueName = uniqueName;
  }

  public string? Id { get; }
  public string? TenantId { get; }
  public string? UniqueName { get; }
}
