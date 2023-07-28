using System.Collections.Immutable;

namespace Logitar.Identity.Domain.Users;

public record Gender
{
  public static IImmutableSet<string> KnownGenders { get; } = ImmutableHashSet.Create(new[] { "female", "male" });

  public Gender(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      throw new ArgumentException("The gender value is required.", nameof(value));
    }

    value = value.Trim();
    if (value.Length > byte.MaxValue)
    {
      throw new ArgumentOutOfRangeException(nameof(value), $"The gender value may contain up to {byte.MaxValue} characters.");
    }

    Value = KnownGenders.Contains(value.ToLower()) ? value.ToLower() : value;
  }

  public string Value { get; }
}
