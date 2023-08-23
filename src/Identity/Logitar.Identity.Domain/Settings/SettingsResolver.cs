namespace Logitar.Identity.Domain.Settings;

public class SettingsResolver : ISettingsResolver
{
  public virtual IRoleSettings RoleSettings { get; } = new RoleSettings();
  public virtual IUserSettings UserSettings { get; } = new UserSettings();

  public virtual IPbkdf2Settings Pbkdf2Settings { get; } = new Pbkdf2Settings();
}
