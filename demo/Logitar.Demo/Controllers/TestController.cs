using Logitar.Demo.Models.Test;
using Microsoft.AspNetCore.Mvc;

namespace Logitar.Demo.Controllers;

[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
  [HttpDelete]
  public async Task<ActionResult<HttpRequestModel>> Delete(CancellationToken cancellationToken)
  {
    return Ok(await HttpRequestModel.FromRequestAsync(Request, cancellationToken));
  }

  [HttpGet]
  public async Task<ActionResult<HttpRequestModel>> Get(CancellationToken cancellationToken)
  {
    return Ok(await HttpRequestModel.FromRequestAsync(Request, cancellationToken));
  }

  [HttpPatch]
  public async Task<ActionResult<HttpRequestModel>> Patch(CancellationToken cancellationToken)
  {
    return Ok(await HttpRequestModel.FromRequestAsync(Request, cancellationToken));
  }

  [HttpPost]
  public async Task<ActionResult<HttpRequestModel>> Post(CancellationToken cancellationToken)
  {
    return Ok(await HttpRequestModel.FromRequestAsync(Request, cancellationToken));
  }

  [HttpPut]
  public async Task<ActionResult<HttpRequestModel>> Put(CancellationToken cancellationToken)
  {
    return Ok(await HttpRequestModel.FromRequestAsync(Request, cancellationToken));
  }
}
