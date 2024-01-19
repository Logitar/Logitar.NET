using Logitar.Demo.Models.Test;

namespace Logitar.Net.Http;

internal record JsonRequestModel
{
  [JsonPropertyName("method")]
  public string Method { get; set; } = string.Empty;

  [JsonPropertyName("url")]
  public string Url { get; set; } = string.Empty;

  [JsonPropertyName("content")]
  public string? Content { get; set; }

  [JsonPropertyName("headers")]
  public List<JsonHeader> Headers { get; set; } = [];

  [JsonPropertyName("authorization")]
  public JsonAuthorization? Authorization { get; set; }

  public HttpRequestModel ToHttpModel()
  {
    HttpRequestModel model = new()
    {
      Method = Method,
      Url = Url,
      Content = Content,
      Authorization = Authorization?.ToHttpAuthorization()
    };

    foreach (JsonHeader header in Headers)
    {
      model.Headers.Add(header.ToHttpHeader());
    }

    return model;
  }
}
