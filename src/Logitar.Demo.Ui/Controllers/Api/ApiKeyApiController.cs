using Logitar.Identity.Core.ApiKeys.Commands;
using Logitar.Identity.Core.ApiKeys.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Logitar.Demo.Ui.Controllers.Api;

[ApiController]
[Route("api/keys")]
public class ApiKeyApiController : ControllerBase
{
  private readonly IMediator _mediator;

  public ApiKeyApiController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost]
  public async Task<ActionResult<ApiKey>> CreateAsync([FromBody] CreateApiKeyPayload payload, CancellationToken cancellationToken)
  {
    ApiKey apiKey = await _mediator.Send(new CreateApiKeyCommand(payload), cancellationToken);
    Uri uri = new($"{Request.Scheme}://{Request.Host}/api/keys/{apiKey.Id}");

    // TODO(fpion): X-API-Key header

    return Created(uri, apiKey);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<ApiKey>> DeleteAsync(string id, CancellationToken cancellationToken)
  {
    ApiKey? apiKey = await _mediator.Send(new DeleteApiKeyCommand(id), cancellationToken);

    return apiKey == null ? NotFound() : Ok(apiKey);
  }
}
