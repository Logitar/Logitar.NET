namespace Logitar.Identity.Core.Settings;

public interface IUserSettings
{
  bool RequireUniqueEmail { get; }
  bool RequireConfirmedAccount { get; }

  IUniqueNameSettings UniqueNameSettings { get; }
  IPasswordSettings PasswordSettings { get; }
}
