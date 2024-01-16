namespace Logitar.Net.Http.Settings;

/// <summary>
/// Defines the settings of a JSON API client.
/// </summary>
public interface IJsonApiSettings : IHttpApiSettings
{
  /// <summary>
  /// Gets the serializer options.
  /// </summary>
  JsonSerializerOptions? SerializerOptions { get; }
}
