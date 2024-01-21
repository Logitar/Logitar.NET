namespace Logitar.Security.Claims;

/// <summary>
/// Defines methods useful to manipulate claims.
/// </summary>
public static class ClaimHelper
{
  /// <summary>
  /// Creates a claim from the specified date and time.
  /// </summary>
  /// <param name="name">The name of the claim.</param>
  /// <param name="moment">The moment value.</param>
  /// <returns>The created claim.</returns>
  public static Claim Create(string name, DateTime moment)
  {
    return Create(name, moment, DateTimeKind.Unspecified);
  }
  /// <summary>
  /// Creates a claim from the specified date and time.
  /// </summary>
  /// <param name="name">The name of the claim.</param>
  /// <param name="moment">The moment value.</param>
  /// <param name="kind">The default date time kind. If unspecified, the moment kind must be specified.</param>
  /// <returns>The created claim.</returns>
  /// <exception cref="ArgumentException">The moment kind and default kind are unspecified.</exception>
  public static Claim Create(string name, DateTime moment, DateTimeKind kind)
  {
    if (moment.Kind == DateTimeKind.Unspecified)
    {
      if (kind == DateTimeKind.Unspecified)
      {
        throw new ArgumentException("The date and time kind must be specified.", nameof(moment));
      }
      else
      {
        moment = DateTime.SpecifyKind(moment, kind);
      }
    }

    string value = new DateTimeOffset(moment.ToUniversalTime()).ToUnixTimeSeconds().ToString();

    return new Claim(name, value, ClaimValueTypes.Integer64);
  }

  /// <summary>
  /// Extracts a date and time from the specified claim.
  /// </summary>
  /// <param name="claim">The claim to extract from.</param>
  /// <returns>The extracted date and time.</returns>
  public static DateTime ExtractDateTime(Claim claim)
  {
    long value = long.Parse(claim.Value);

    return DateTime.SpecifyKind(DateTimeOffset.FromUnixTimeSeconds(value).DateTime, DateTimeKind.Utc);
  }
}

// TODO(fpion): refactor to use DateTimeHelper
