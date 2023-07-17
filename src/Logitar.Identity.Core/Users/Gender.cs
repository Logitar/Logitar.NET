namespace Logitar.Identity.Core.Users;

/// <summary>
/// Represents the gender of an user. See <see href="https://openid.net/specs/openid-connect-core-1_0.html"/>,
/// Section 5.1: Standard Claims, for more detail.
/// </summary>
public record Gender
{
  /// <summary>
  /// Initializes a new instance of the <see cref="Gender"/> class.
  /// </summary>
  /// <param name="value">The string value of the gender.</param>
  /// <exception cref="ArgumentException">The string value is empty or only white space.</exception>
  /// <exception cref="ArgumentOutOfRangeException">The string value is too long.</exception>
  public Gender(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      throw new ArgumentException("The gender value is required.", nameof(value));
    }

    value = value.Trim();
    if (value.Length > byte.MaxValue)
    {
      throw new ArgumentOutOfRangeException(nameof(value), $"The gender may contain up to {byte.MaxValue} characters.");
    }

    Value = KnownGenders.Contains(value.ToLower()) ? value.ToLower() : value;
  }

  /// <summary>
  /// Gets the known gender values. Other values may be used when neither of the defined values are applicable.
  /// </summary>
  public static ISet<string> KnownGenders => ImmutableHashSet.Create(new[] { "female", "male" });

  /// <summary>
  /// Gets the string value of the gender.
  /// </summary>
  public string Value { get; }
}
