using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Logitar.Security;

/// <summary>
/// TODO(fpion): documentation
/// TODO(fpion): tests
/// Reference: <see href="https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html#pbkdf2"/>
/// </summary>
public class Pbkdf2
{
  public const string Prefix = "PBKDF2";
  public const char Separator = ':';

  private readonly KeyDerivationPrf _algorithm;
  private readonly int _iterations;
  private readonly byte[] _salt;
  private readonly byte[] _hash;

  public Pbkdf2(byte[] password) : this(Convert.ToBase64String(password))
  {
  }
  public Pbkdf2(string password)
  {
    _algorithm = KeyDerivationPrf.HMACSHA256;
    _iterations = 600000;
    _salt = RandomNumberGenerator.GetBytes(256 / 8);
    _hash = ComputeHash(password, _salt.Length);
  }

  private Pbkdf2(KeyDerivationPrf algorithm, int iterations, byte[] salt, byte[] hash)
  {
    _algorithm = algorithm;
    _iterations = iterations;
    _salt = salt;
    _hash = hash;
  }

  public static Pbkdf2 Parse(string s)
  {
    string[] values = s.Split(Separator);
    if (values.Length != 5 || values[0] != Prefix)
    {
      throw new ArgumentException($"The value '{s}' is not a valid string representation of PBKDF2.", nameof(s));
    }

    return new Pbkdf2(Enum.Parse<KeyDerivationPrf>(values[1]), int.Parse(values[2]),
      Convert.FromBase64String(values[3]), Convert.FromBase64String(values[4]));
  }
  public static bool TryParse(string s, out Pbkdf2? pbkdf2)
  {
    try
    {
      pbkdf2 = Parse(s);
      return true;
    }
    catch (Exception)
    {
      pbkdf2 = null;
      return false;
    }
  }

  public bool IsMatch(byte[] password) => IsMatch(Convert.ToBase64String(password));
  public bool IsMatch(string password) => _hash.SequenceEqual(ComputeHash(password));

  private byte[] ComputeHash(string password, int? length = null)
    => KeyDerivation.Pbkdf2(password, _salt, _algorithm, _iterations, length ?? _hash.Length);

  public override bool Equals(object? obj) => obj is Pbkdf2 other && other._algorithm == _algorithm
    && other._iterations == _iterations && other._salt.SequenceEqual(_salt) && other._hash.SequenceEqual(_hash);
  public override int GetHashCode() => HashCode.Combine(Prefix, _algorithm, _iterations,
    Convert.ToBase64String(_salt), Convert.ToBase64String(_hash));
  public override string ToString() => string.Join(Separator, Prefix, _algorithm, _iterations,
    Convert.ToBase64String(_salt), Convert.ToBase64String(_hash));
}
