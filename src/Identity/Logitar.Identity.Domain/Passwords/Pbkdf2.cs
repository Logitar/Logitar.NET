using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Logitar.Identity.Domain.Passwords;

public record Pbkdf2 : Password
{
  public const string Prefix = "PBKDF2";

  private readonly KeyDerivationPrf _algorithm;
  private readonly int _iterations;
  private readonly byte[] _salt;
  private readonly byte[] _hash;

  public Pbkdf2(string password, KeyDerivationPrf algorithm, int iterations, byte[] salt, int? hashLength = null)
  {
    _algorithm = algorithm;
    _iterations = iterations;
    _salt = salt;
    _hash = ComputeHash(password, hashLength ?? salt.Length);
  }

  private Pbkdf2(KeyDerivationPrf algorithm, int iterations, byte[] salt, byte[] hash)
  {
    _algorithm = algorithm;
    _iterations = iterations;
    _salt = salt;
    _hash = hash;
  }

  public static Pbkdf2 Decode(string encoded)
  {
    string[] values = encoded.Split(Separator);
    if (values.Length != 5 || values.First() != Prefix)
    {
      throw new ArgumentException($"The value '{encoded}' is not a valid encoded PBKDF2 password.", nameof(encoded));
    }

    return new Pbkdf2(Enum.Parse<KeyDerivationPrf>(values[1]), int.Parse(values[2]),
      Convert.FromBase64String(values[3]), Convert.FromBase64String(values[4]));
  }

  public override string Encode() => string.Join(Separator, Prefix, _algorithm, _iterations,
    Convert.ToBase64String(_salt), Convert.ToBase64String(_hash));

  public override bool IsMatch(string password) => _hash.SequenceEqual(ComputeHash(password));

  private byte[] ComputeHash(string password, int? length = null)
    => KeyDerivation.Pbkdf2(password, _salt, _algorithm, _iterations, length ?? _hash.Length);
}
