namespace Logitar.Identity.Core.Settings;

/// <summary>
/// The settings used to validate unique names.
/// </summary>
public record UniqueNameSettings : IUniqueNameSettings
{
  /// <summary>
  /// Gets a string containing the list of allowed characters.
  /// </summary>
  public string? AllowedCharacters { get; set; }
}
