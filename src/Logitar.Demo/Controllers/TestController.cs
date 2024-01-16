using Logitar.Demo.Models.Request;
using Logitar.Net.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Logitar.Demo.Controllers;

[ApiController]
[Route("request")]
public class TestController : ControllerBase
{
  [HttpDelete]
  public async Task<ActionResult<HttpRequestModel>> Delete(CancellationToken cancellationToken)
  {
    return Ok(await GetRequestAsync(cancellationToken));
  }

  [HttpGet]
  public async Task<ActionResult<HttpRequestModel>> Get(CancellationToken cancellationToken)
  {
    return Ok(await GetRequestAsync(cancellationToken));
  }

  [HttpPatch]
  public async Task<ActionResult<HttpRequestModel>> Patch(CancellationToken cancellationToken)
  {
    return Ok(await GetRequestAsync(cancellationToken));
  }

  [HttpPost]
  public async Task<ActionResult<HttpRequestModel>> Post(CancellationToken cancellationToken)
  {
    return Ok(await GetRequestAsync(cancellationToken));
  }

  [HttpPut]
  public async Task<ActionResult<HttpRequestModel>> Put(CancellationToken cancellationToken)
  {
    return Ok(await GetRequestAsync(cancellationToken));
  }

  private async Task<HttpRequestModel> GetRequestAsync(CancellationToken cancellationToken)
  {
    HttpRequestModel request = new()
    {
      Method = Request.Method,
      Url = Request.GetDisplayUrl()
    };

    using StreamReader reader = new(Request.Body);
    request.Content = (await reader.ReadToEndAsync(cancellationToken)).CleanTrim();

    foreach (KeyValuePair<string, StringValues> header in Request.Headers)
    {
      List<string> values = new(capacity: header.Value.Count);
      foreach (string? value in header.Value)
      {
        if (value != null)
        {
          values.Add(value);
        }
      }

      request.Headers.Add(new HttpHeader(header.Key, values));
    }

    return request;
  }
}
