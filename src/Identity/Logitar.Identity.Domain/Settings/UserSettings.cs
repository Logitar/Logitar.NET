namespace Logitar.Identity.Domain.Settings;

public record UserSettings : IUserSettings
{
  public bool RequireUniqueEmail { get; set; } = true;
  public bool RequireConfirmedAccount { get; set; } = true;

  public IUniqueNameSettings UniqueNameSettings { get; set; } = new UniqueNameSettings();
  public IPasswordSettings PasswordSettings { get; set; } = new PasswordSettings();
}
