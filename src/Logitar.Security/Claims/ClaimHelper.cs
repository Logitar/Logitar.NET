﻿namespace Logitar.Security.Claims;

/// <summary>
/// Defines methods useful to manipulate claims.
/// </summary>
public static class ClaimHelper
{
  /// <summary>
  /// Creates a claim from the specified date and time.
  /// </summary>
  /// <param name="name">The name of the claim.</param>
  /// <param name="moment">The moment value. If the date time kind is Unspecified, the date time will be treated as Local.</param>
  /// <returns>The created claim.</returns>
  public static Claim Create(string name, DateTime moment)
  {
    string value = new DateTimeOffset(moment.AsUniversalTime()).ToUnixTimeSeconds().ToString();

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
