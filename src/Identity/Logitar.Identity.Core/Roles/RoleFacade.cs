using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Roles.Commands;
using Logitar.Identity.Core.Roles.Models;
using Logitar.Identity.Core.Roles.Payloads;
using Logitar.Identity.Core.Roles.Queries;
using MediatR;

namespace Logitar.Identity.Core.Roles;

public class RoleFacade : IRoleFacade
{
  private readonly IMediator _mediator;

  public RoleFacade(IMediator mediator)
  {
    _mediator = mediator;
  }

  public virtual async Task<Role> CreateAsync(CreateRolePayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new CreateRoleCommand(payload), cancellationToken);
  }

  public virtual async Task<Role?> DeleteAsync(string id, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new DeleteRoleCommand(id), cancellationToken);
  }

  public virtual async Task<Role?> ReadAsync(string? id, string? tenantId, string? uniqueName, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new ReadRoleQuery(id, tenantId, uniqueName), cancellationToken);
  }

  public virtual async Task<Role?> ReplaceAsync(string id, ReplaceRolePayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new ReplaceRoleCommand(id, payload), cancellationToken);
  }

  public virtual async Task<SearchResults<Role>> SearchAsync(SearchRolePayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new SearchRoleQuery(payload), cancellationToken);
  }

  public virtual async Task<Role?> UpdateAsync(string id, UpdateRolePayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new UpdateRoleCommand(id, payload), cancellationToken);
  }
}
