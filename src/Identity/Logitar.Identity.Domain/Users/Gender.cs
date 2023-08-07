using System.Collections.Immutable;

namespace Logitar.Identity.Domain.Users;

public record Gender
{
  private const int MaximumLength = byte.MaxValue;

  public static readonly IImmutableSet<string> KnownValues = ImmutableHashSet.Create(new[] { "female", "male" });
  public static string? GetKnownValue(string value) => KnownValues.Contains(value.ToLower()) ? value.ToLower() : null;

  public Gender(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      throw new ArgumentException("The gender value is required.", nameof(value));
    }

    value = value.Trim();
    if (value.Length > MaximumLength)
    {
      throw new ArgumentOutOfRangeException(nameof(value), $"The value cannot exceed {MaximumLength} characters.");
    }

    Value = GetKnownValue(value) ?? value;
  }

  public string Value { get; }
}
