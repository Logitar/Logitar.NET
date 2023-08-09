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

  public override string Encode() => string.Join(Separator, Prefix, _algorithm, _iterations,
    Convert.ToBase64String(_salt), Convert.ToBase64String(_hash));

  public override bool IsMatch(string password) => _hash.SequenceEqual(ComputeHash(password));

  private byte[] ComputeHash(string password, int? length = null)
    => KeyDerivation.Pbkdf2(password, _salt, _algorithm, _iterations, length ?? _hash.Length);
}
