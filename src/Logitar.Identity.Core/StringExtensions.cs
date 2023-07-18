using FluentValidation;
using FluentValidation.Results;
using Logitar.EventSourcing;
using Logitar.Identity.Core.Users;

namespace Logitar.Identity.Core;

/// <summary>
/// Provides extension methods for <see cref="string"/> instances.
/// </summary>
public static class StringExtensions
{
  /// <summary>
  /// Tries parsing an <see cref="AggregateId"/> from the specified input string.
  /// </summary>
  /// <param name="input">The input string.</param>
  /// <param name="propertyName">The name of the property.</param>
  /// <returns>The parsed instance.</returns>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public static AggregateId? ParseAggregateId(this string input, string propertyName)
  {
    if (string.IsNullOrWhiteSpace(input))
    {
      return null;
    }

    try
    {
      return new AggregateId(input);
    }
    catch (Exception innerException)
    {
      throw new ValidationException(new[]
      {
        new ValidationFailure(propertyName, errorMessage: innerException.Message, input)
        {
          ErrorCode = "InvalidAggregateId"
        }
      });
    }
  }
  /// <summary>
  /// Parses a required <see cref="AggregateId"/> from the specified input string.
  /// </summary>
  /// <param name="input">The input string.</param>
  /// <param name="propertyName">The name of the property.</param>
  /// <returns>The parsed instance.</returns>
  public static AggregateId RequireAggregateId(this string input, string propertyName)
  {
    return ParseAggregateId(input, propertyName) ?? throw new ValidationException(new[]
    {
      new ValidationFailure(propertyName, errorMessage: "The aggregate identifier cannot be null, empty, or only white space.", input)
      {
        ErrorCode = "RequiredAggregateId"
      }
    });
  }

  /// <summary>
  /// Tries parsing a <see cref="Gender"/> from the specified input string.
  /// </summary>
  /// <param name="input">The input string.</param>
  /// <param name="propertyName">The name of the property.</param>
  /// <returns>The parsed instance.</returns>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public static Gender? ParseGender(this string input, string propertyName)
  {
    if (string.IsNullOrWhiteSpace(input))
    {
      return null;
    }

    try
    {
      return new Gender(input);
    }
    catch (Exception innerException)
    {
      throw new ValidationException(new[]
      {
        new ValidationFailure(propertyName, errorMessage: innerException.Message, input)
        {
          ErrorCode = "InvalidGender"
        }
      });
    }
  }

  /// <summary>
  /// Tries parsing a <see cref="CultureInfo"/> from the specified input string.
  /// </summary>
  /// <param name="input">The input string.</param>
  /// <param name="propertyName">The name of the property.</param>
  /// <returns>The parsed instance.</returns>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public static CultureInfo? ParseLocale(this string input, string propertyName)
  {
    if (string.IsNullOrWhiteSpace(input))
    {
      return null;
    }

    try
    {
      return CultureInfo.GetCultureInfo(input);
    }
    catch (Exception innerException)
    {
      throw new ValidationException(new[]
      {
        new ValidationFailure(propertyName, errorMessage: innerException.Message, input)
        {
          ErrorCode = "InvalidLocale"
        }
      });
    }
  }

  /// <summary>
  /// Tries parsing a <see cref="TimeZone"/> from the specified input string.
  /// </summary>
  /// <param name="input">The input string.</param>
  /// <param name="propertyName">The name of the property.</param>
  /// <returns>The parsed instance.</returns>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public static Users.TimeZone? ParseTimeZone(this string input, string propertyName)
  {
    if (string.IsNullOrWhiteSpace(input))
    {
      return null;
    }

    try
    {
      return new Users.TimeZone(input);
    }
    catch (Exception innerException)
    {
      throw new ValidationException(new[]
      {
        new ValidationFailure(propertyName, errorMessage: innerException.Message, input)
        {
          ErrorCode = "InvalidTimeZone"
        }
      });
    }
  }

  /// <summary>
  /// Tries parsing an <see cref="Uri"/> from the specified input string.
  /// </summary>
  /// <param name="input">The input string.</param>
  /// <param name="propertyName">The name of the property.</param>
  /// <returns>The parsed instance.</returns>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public static Uri? ParseUri(this string input, string propertyName)
  {
    if (string.IsNullOrWhiteSpace(input))
    {
      return null;
    }

    try
    {
      return new Uri(input);
    }
    catch (Exception innerException)
    {
      throw new ValidationException(new[]
      {
        new ValidationFailure(propertyName, errorMessage: innerException.Message, input)
        {
          ErrorCode = "InvalidUri"
        }
      });
    }
  }
}
