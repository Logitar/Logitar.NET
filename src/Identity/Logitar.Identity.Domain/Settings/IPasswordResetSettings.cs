namespace Logitar.Identity.Domain.Settings;

public interface IPasswordResetSettings
{
  int Lifetime { get; }
  string Purpose { get; }
}
