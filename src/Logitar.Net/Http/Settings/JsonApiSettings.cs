namespace Logitar.Net.Http.Settings;

/// <summary>
/// Implements the settings of a JSON API client.
/// </summary>
public record JsonApiSettings : HttpApiSettings, IJsonApiSettings
{
  /// <summary>
  /// Gets the serializer options.
  /// </summary>
  public JsonSerializerOptions? SerializerOptions { get; set; }
}
