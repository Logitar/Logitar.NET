namespace Logitar.Identity.Domain.Settings;

public record UserSettings : IUserSettings
{
  public bool RequireUniqueEmail { get; set; } = true;
  public bool RequireConfirmedAccount { get; set; } = true;

  public IUniqueNameSettings UniqueNameSettings { get; set; } = new UniqueNameSettings
  {
    AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"
  };
  public IPasswordSettings PasswordSettings { get; set; } = new PasswordSettings
  {
    RequiredLength = 8,
    RequiredUniqueChars = 8,
    RequireNonAlphanumeric = true,
    RequireLowercase = true,
    RequireUppercase = true,
    RequireDigit = true
  };

  public IPasswordResetSettings PasswordResetSettings { get; set; } = new PasswordResetSettings();
}
