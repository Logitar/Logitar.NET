using Logitar.Net.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;

namespace Logitar.Demo.Models.Test;

public record HttpRequestModel
{
  public string Method { get; set; } = string.Empty;
  public string Url { get; set; } = string.Empty;
  public string? Content { get; set; }
  public List<HttpHeader> Headers { get; set; } = [];
  public HttpAuthorization? Authorization { get; set; }

  public static async Task<HttpRequestModel> FromRequestAsync(HttpRequest request, CancellationToken cancellationToken)
  {
    HttpRequestModel model = new()
    {
      Method = request.Method,
      Url = request.GetDisplayUrl()
    };

    using StreamReader reader = new(request.Body);
    model.Content = (await reader.ReadToEndAsync(cancellationToken)).CleanTrim();

    foreach (KeyValuePair<string, StringValues> header in request.Headers)
    {
      List<string> values = new(header.Value.Count);
      foreach (string? value in header.Value)
      {
        if (value != null)
        {
          values.Add(value);
        }
      }
      model.Headers.Add(new HttpHeader(header.Key, values));
    }

    if (request.Headers.Authorization.Count == 1)
    {
      string[] parts = request.Headers.Authorization.Single()?.Split(' ') ?? [];
      if (parts.Length == 2)
      {
        model.Authorization = new HttpAuthorization(parts[0], parts[1]);
      }
    }

    return model;
  }
}
