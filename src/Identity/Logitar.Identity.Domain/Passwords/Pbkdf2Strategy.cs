using Logitar.Identity.Domain.Settings;

namespace Logitar.Identity.Domain.Passwords;

public class Pbkdf2Strategy : IPasswordStrategy
{
  public string Id => Pbkdf2.Prefix;

  private readonly ISettingsResolver _settingsResolver;

  public Pbkdf2Strategy(ISettingsResolver settingsResolver)
  {
    _settingsResolver = settingsResolver;
  }

  public Password Create(string password)
  {
    IPbkdf2Settings pbkdf2Settings = _settingsResolver.Pbkdf2Settings;

    byte[] salt = RandomNumberGenerator.GetBytes(pbkdf2Settings.SaltLength);

    return new Pbkdf2(password, pbkdf2Settings.Algorithm, pbkdf2Settings.Iterations, salt, pbkdf2Settings.HashLength);
  }

  public Password Decode(string encoded) => Pbkdf2.Decode(encoded);
}
