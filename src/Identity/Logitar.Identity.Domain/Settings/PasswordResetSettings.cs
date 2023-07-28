namespace Logitar.Identity.Domain.Settings;

public record PasswordResetSettings : IPasswordResetSettings
{
  public int Lifetime { get; } = 60 * 60;
  public string Purpose { get; } = "reset_password";
}
