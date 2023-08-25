namespace Logitar.Identity.Domain.Settings;

public interface ISettingsResolver
{
  IRoleSettings RoleSettings { get; }
  IUserSettings UserSettings { get; }

  IPbkdf2Settings Pbkdf2Settings { get; }
}
