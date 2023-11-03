namespace Logitar.Security.Cryptography;

/// <summary>
/// Provides functionality for generating random values.
/// </summary>
public abstract class RandomStringGenerator
{
  /// <summary>
  /// Initializes a new instance of the <see cref="RandomStringGenerator"/> class.
  /// </summary>
  protected RandomStringGenerator()
  {
  }

  /// <summary>
  /// Creates a string with a cryptographically strong random sequence of characters.
  /// </summary>
  /// <param name="count">The number of characters of random values to create. Defaults to 32 characters (256 bits).</param>
  /// <returns>A string populated with cryptographically strong random characters.</returns>
  public static string GetString(int count = 256 / 8)
  {
    while (true)
    {
      /* In the ASCII table, there are 94 characters between 33 '!' and 126 '~' (126 - 33 + 1 = 94).
       * Since there are a total of 256 possibilities, by dividing per 94 and adding a 10% margin we
       * generate just a little more bytes than required, obtaining the factor 3. */
      byte[] bytes = RandomNumberGenerator.GetBytes(count * 3);

      List<byte> secret = new(capacity: count);
      for (int i = 0; i < bytes.Length; i++)
      {
        byte @byte = bytes[i];
        if (@byte >= 33 && @byte <= 126)
        {
          secret.Add(@byte);

          if (secret.Count == count)
          {
            return Encoding.ASCII.GetString(secret.ToArray());
          }
        }
      }
    }
  }
}
