using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Logitar.Security.Cryptography;

/// <summary>
/// Represents an implementation of the Password-Based Key Derivation Function 2 (PBDKF2), that is
/// used to store salted and hashed passwords.
/// <br /><see href="https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html#pbkdf2"/>
/// <br />Last consulted on 2023-07-13 12:00:00 (UTC)
/// </summary>
public class Pbkdf2
{
  /// <summary>
  /// The separator character between the parts of a PBKDF2 string representation.
  /// </summary>
  public const char Separator = ':';

  /// <summary>
  /// The hashing algorithm of the password.
  /// </summary>
  private readonly KeyDerivationPrf _algorithm;
  /// <summary>
  /// The number of iterations of the password.
  /// </summary>
  private readonly int _iterations;
  /// <summary>
  /// The salt of the password.
  /// </summary>
  private readonly byte[] _salt;
  /// <summary>
  /// The hash of the password.
  /// </summary>
  private readonly byte[] _hash;

  /// <summary>
  /// Initializes a new instance of the <see cref="Pbkdf2"/> class.
  /// </summary>
  /// <param name="password">The password bytes to use.</param>
  /// <param name="algorithm">The hashing algorithm to use.</param>
  /// <param name="iterations">The number of iterations to use.</param>
  /// <param name="saltLength">The desired length of the salt.</param>
  /// <param name="hashLength">The desired length of the hash.</param>
  public Pbkdf2(byte[] password, KeyDerivationPrf algorithm = KeyDerivationPrf.HMACSHA256,
    int iterations = 600000, int saltLength = 32, int? hashLength = null)
      : this(Convert.ToBase64String(password), algorithm, iterations, saltLength, hashLength)
  {
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="Pbkdf2"/> class.
  /// </summary>
  /// <param name="password">The password string to use.</param>
  /// <param name="algorithm">The hashing algorithm to use.</param>
  /// <param name="iterations">The number of iterations to use.</param>
  /// <param name="saltLength">The desired length of the salt.</param>
  /// <param name="hashLength">The desired length of the hash.</param>
  public Pbkdf2(string password, KeyDerivationPrf algorithm = KeyDerivationPrf.HMACSHA256,
    int iterations = 600000, int saltLength = 32, int? hashLength = null)
  {
    _algorithm = algorithm;
    _iterations = iterations;
    _salt = RandomNumberGenerator.GetBytes(saltLength);
    _hash = ComputeHash(password, hashLength ?? saltLength);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="Pbkdf2"/> class.
  /// </summary>
  /// <param name="algorithm">The hashing algorithm of the password.</param>
  /// <param name="iterations">The number of iterations of the password.</param>
  /// <param name="salt">The salt of the password.</param>
  /// <param name="hash">The hash of the password.</param>
  private Pbkdf2(KeyDerivationPrf algorithm, int iterations, byte[] salt, byte[] hash)
  {
    _algorithm = algorithm;
    _iterations = iterations;
    _salt = salt;
    _hash = hash;
  }

  /// <summary>
  /// Parses the specified string representation to an instance of PBKDF2.
  /// </summary>
  /// <param name="s">The string representation to parse.</param>
  /// <returns>The instance of PBKDF2.</returns>
  /// <exception cref="ArgumentException">The string representation was not a valid PBKDF2.</exception>
  public static Pbkdf2 Parse(string s)
  {
    string[] parts = s.Split(Separator);
    if (parts.Length != 4)
    {
      throw new ArgumentException($"The value '{s}' is not a valid PBKDF2 string representation.", nameof(s));
    }

    return new Pbkdf2(Enum.Parse<KeyDerivationPrf>(parts[0]), int.Parse(parts[1]),
      Convert.FromBase64String(parts[2]), Convert.FromBase64String(parts[3]));
  }
  /// <summary>
  /// Tries parsing the specified string representation to an instance of PBKDF2.
  /// </summary>
  /// <param name="s">The string representation to parse.</param>
  /// <param name="pbkdf2">The instance of PBKDF2.</param>
  /// <returns>A value indicating whether or not the string representation have been parsed to an instance of PBKDF2.</returns>
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

  /// <summary>
  /// Returns a value indicating whether or not the specified bytes password matches the current instance of PBKDF2.
  /// </summary>
  /// <param name="password">The bytes password to match against.</param>
  /// <returns>A value indicating whether or not it is a match.</returns>
  public bool IsMatch(byte[] password) => IsMatch(Convert.ToBase64String(password));
  /// <summary>
  /// Returns a value indicating whether or not the specified string password matches the current instance of PBKDF2.
  /// </summary>
  /// <param name="password">The string password to match against.</param>
  /// <returns>A value indicating whether or not it is a match.</returns>
  public bool IsMatch(string password) => _hash.SequenceEqual(ComputeHash(password, _hash.Length));

  /// <summary>
  /// Computes the hash of the specified password, to the specified length, using the current PBKDF2 instance values.
  /// </summary>
  /// <param name="password">The password to compute its hash.</param>
  /// <param name="hashLength">The desired length of the hash.</param>
  /// <returns>The computed hash bytes.</returns>
  private byte[] ComputeHash(string password, int hashLength)
  {
    return KeyDerivation.Pbkdf2(password, _salt, _algorithm, _iterations, hashLength);
  }

  /// <summary>
  /// Returns a value indicating whether or not the specified object is equal to the PBKDF2.
  /// </summary>
  /// <param name="obj">The object to be compared to.</param>
  /// <returns>True if the object is equal to the PBKDF2.</returns>
  public override bool Equals(object? obj) => obj is Pbkdf2 pbkdf2 && pbkdf2._algorithm == _algorithm
    && pbkdf2._iterations == _iterations && pbkdf2._salt.SequenceEqual(_salt) && pbkdf2._hash.SequenceEqual(_hash);
  /// <summary>
  /// Returns the hash code of the current PBKDF2.
  /// </summary>
  /// <returns>The hash code.</returns>
  public override int GetHashCode() => HashCode.Combine(_algorithm, _iterations,
    Convert.ToBase64String(_salt), Convert.ToBase64String(_hash));
  /// <summary>
  /// Returns a string representation of the PBKDF2.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => string.Join(Separator, _algorithm, _iterations,
    Convert.ToBase64String(_salt), Convert.ToBase64String(_hash));
}
