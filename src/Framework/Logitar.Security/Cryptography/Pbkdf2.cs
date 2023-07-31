using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Logitar.Security.Cryptography;

/// <summary>
/// TODO(fpion): documentation
/// TODO(fpion): tests
/// Reference: <see href="https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html#pbkdf2"/>
/// </summary>
public record Pbkdf2 : Password
{
  public const string Prefix = "PBKDF2";

  private readonly KeyDerivationPrf _algorithm;
  private readonly int _iterations;
  private readonly byte[] _salt;
  private readonly byte[] _hash;

  public Pbkdf2(byte[] password, Pbkdf2Settings? settings = null)
    : this(Convert.ToBase64String(password), settings)
  {
  }
  public Pbkdf2(string password, Pbkdf2Settings? settings = null)
  {
    settings ??= new();
    byte[] salt = RandomNumberGenerator.GetBytes(settings.SaltLength);

    _algorithm = settings.Algorithm;
    _iterations = settings.Iterations;
    _salt = salt;
    _hash = ComputeHash(password, settings.HashLength ?? salt.Length);
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
    var values = encoded.Split(Separator);
    if (values.Length != 5 || values[0] != Prefix)
    {
      throw new ArgumentException($"The value '{encoded}' is not a valid string representation of PBKDF2.", nameof(encoded));
    }

    return new Pbkdf2(Enum.Parse<KeyDerivationPrf>(values[1]), int.Parse(values[2]),
      Convert.FromBase64String(values[3]), Convert.FromBase64String(values[4]));
  }
  public static bool TryDecode(string encoded, out Pbkdf2? pbkdf2)
  {
    try
    {
      pbkdf2 = Decode(encoded);
      return true;
    }
    catch (Exception)
    {
      pbkdf2 = null;
      return false;
    }
  }

  public override string Encode() => string.Join(Separator, Prefix, _algorithm, _iterations,
    Convert.ToBase64String(_salt), Convert.ToBase64String(_hash));

  public override bool IsMatch(byte[] password) => IsMatch(Convert.ToBase64String(password));
  public override bool IsMatch(string password) => _hash.SequenceEqual(ComputeHash(password));

  private byte[] ComputeHash(string password, int? length = null)
    => KeyDerivation.Pbkdf2(password, _salt, _algorithm, _iterations, length ?? _hash.Length);
}
