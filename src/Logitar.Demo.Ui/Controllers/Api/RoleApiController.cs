using Logitar.Identity.Core.Roles.Commands;
using Logitar.Identity.Core.Roles.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Logitar.Demo.Ui.Controllers.Api;

[ApiController]
[Route("api/roles")]
public class RoleApiController : ControllerBase
{
  private readonly IMediator _mediator;

  public RoleApiController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost]
  public async Task<ActionResult<Role>> CreateAsync([FromBody] CreateRolePayload payload, CancellationToken cancellationToken)
  {
    Role role = await _mediator.Send(new CreateRoleCommand(payload), cancellationToken);
    Uri uri = new($"{Request.Scheme}://{Request.Host}/api/roles/{role.Id}");

    return Created(uri, role);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<Role>> DeleteAsync(string id, CancellationToken cancellationToken)
  {
    Role? role = await _mediator.Send(new DeleteRoleCommand(id), cancellationToken);

    return role == null ? NotFound() : Ok(role);
  }
}
