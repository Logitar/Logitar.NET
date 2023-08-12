using Logitar.Identity.Domain.Settings;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Domain.Passwords;

public class Pbkdf2Strategy : IPasswordStrategy
{
  public string Id => Pbkdf2.Prefix;

  private readonly IOptions<Pbkdf2Settings> _pbkdf2Settings;

  public Pbkdf2Strategy(IOptions<Pbkdf2Settings> pbkdf2Settings)
  {
    _pbkdf2Settings = pbkdf2Settings;
  }

  public Password Create(string password)
  {
    Pbkdf2Settings pbkdf2Settings = _pbkdf2Settings.Value;

    byte[] salt = RandomNumberGenerator.GetBytes(pbkdf2Settings.SaltLength);

    return new Pbkdf2(password, pbkdf2Settings.Algorithm, pbkdf2Settings.Iterations, salt, pbkdf2Settings.HashLength);
  }

  public Password Decode(string encoded) => Pbkdf2.Decode(encoded);
}
