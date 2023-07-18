namespace Logitar.Identity.Core.Settings;

/// <summary>
/// The settings used to validate roles.
/// </summary>
public interface IRoleSettings
{
  /// <summary>
  /// Gets the settings used to validate role unique names.
  /// </summary>
  IUniqueNameSettings UniqueNameSettings { get; }
}
