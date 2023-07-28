using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Roles.Models;
using Logitar.Identity.Core.Roles.Payloads;
using MediatR;

namespace Logitar.Identity.Core.Roles.Queries;

public record SearchRoleQuery : IRequest<SearchResults<Role>>
{
  public SearchRoleQuery(SearchRolePayload payload)
  {
    Payload = payload;
  }

  public SearchRolePayload Payload { get; }
}
