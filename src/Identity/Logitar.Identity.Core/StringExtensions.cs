using FluentValidation;
using FluentValidation.Results;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Core;

/// <summary>
/// TODO(fpion): Get or Parse?
/// </summary>
public static class StringExtensions
{
  public static AggregateId GetAggregateId(this string value, string propertyName)
  {
    try
    {
      return new AggregateId(value);
    }
    catch (Exception exception)
    {
      throw new ValidationException(new[]
      {
        new ValidationFailure(propertyName, exception.Message, value)
        {
          ErrorCode = "InvalidAggregateId"
        }
      });
    }
  }

  public static CultureInfo? GetCultureInfo(this string? name, string propertyName)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      return null;
    }

    try
    {
      return CultureInfo.GetCultureInfo(name.Trim());
    }
    catch (Exception exception)
    {
      throw new ValidationException(new[]
      {
        new ValidationFailure(propertyName, exception.Message, name)
        {
          ErrorCode = "InvalidCultureInfo"
        }
      });
    }
  }

  public static Gender? GetGender(this string? value, string propertyName)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      return null;
    }

    try
    {
      return new Gender(value.Trim());
    }
    catch (Exception exception)
    {
      throw new ValidationException(new[]
      {
        new ValidationFailure(propertyName, exception.Message, value)
        {
          ErrorCode = "InvalidGender"
        }
      });
    }
  }

  public static TimeZoneEntry? GetTimeZone(this string? id, string propertyName)
  {
    if (string.IsNullOrWhiteSpace(id))
    {
      return null;
    }

    try
    {
      return new TimeZoneEntry(id.Trim());
    }
    catch (Exception exception)
    {
      throw new ValidationException(new[]
      {
        new ValidationFailure(propertyName, exception.Message, id)
        {
          ErrorCode = "InvalidTimeZone"
        }
      });
    }
  }

  public static Uri? GetUri(this string? uriString, string propertyName)
  {
    if (string.IsNullOrWhiteSpace(uriString))
    {
      return null;
    }

    try
    {
      return new Uri(uriString.Trim());
    }
    catch (Exception exception)
    {
      throw new ValidationException(new[]
      {
        new ValidationFailure(propertyName, exception.Message, uriString)
        {
          ErrorCode = "InvalidUri"
        }
      });
    }
  }
}
