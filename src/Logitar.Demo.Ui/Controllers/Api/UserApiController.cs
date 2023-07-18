using Logitar.Identity.Core.Users.Commands;
using Logitar.Identity.Core.Users.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Logitar.Demo.Ui.Controllers.Api;

[ApiController]
[Route("api/users")]
public class UserApiController : ControllerBase
{
  private readonly IMediator _mediator;

  public UserApiController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost]
  public async Task<ActionResult<User>> CreateAsync([FromBody] CreateUserPayload payload, CancellationToken cancellationToken)
  {
    User user = await _mediator.Send(new CreateUserCommand(payload), cancellationToken);
    Uri uri = new($"{Request.Scheme}://{Request.Host}/api/users/{user.Id}");

    return Created(uri, user);
  }
}
