namespace Logitar.Identity.Core.Settings;

/// <summary>
/// The settings used to validate roles.
/// </summary>
public record RoleSettings : IRoleSettings
{
  /// <summary>
  /// Gets or sets the settings used to validate role unique names.
  /// </summary>
  public IUniqueNameSettings UniqueNameSettings { get; set; } = new UniqueNameSettings();
}
