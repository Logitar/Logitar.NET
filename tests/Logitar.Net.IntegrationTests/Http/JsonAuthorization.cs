namespace Logitar.Net.Http;

internal record JsonAuthorization
{
  [JsonPropertyName("scheme")]
  public string Scheme { get; set; } = string.Empty;

  [JsonPropertyName("credentials")]
  public string Credentials { get; set; } = string.Empty;

  public HttpAuthorization ToHttpAuthorization() => new(Scheme, Credentials);
}
