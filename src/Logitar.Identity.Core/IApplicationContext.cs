using Logitar.Identity.Core.Settings;

namespace Logitar.Identity.Core;

/// <summary>
/// Represents the context of an application using the identity system.
/// </summary>
public interface IApplicationContext
{
  /// <summary>
  /// Gets the role settings of the application.
  /// </summary>
  IRoleSettings RoleSettings { get; }
  /// <summary>
  /// Gets the user settings of the application.
  /// </summary>
  IUserSettings UserSettings { get; }
}
