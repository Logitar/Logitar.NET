using Logitar.Net.Http;

namespace Logitar.Demo.Models.Request;

public record HttpRequestModel
{
  public string Method { get; set; } = string.Empty;
  public string Url { get; set; } = string.Empty;
  public string? Content { get; set; }
  public List<HttpHeader> Headers { get; set; } = [];
}
