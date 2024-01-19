namespace Logitar.Net.Http;

internal record JsonHeader
{
  [JsonPropertyName("name")]
  public string Name { get; set; } = string.Empty;

  [JsonPropertyName("values")]
  public List<string> Values { get; set; } = [];

  public HttpHeader ToHttpHeader() => new(Name, Values);
}
