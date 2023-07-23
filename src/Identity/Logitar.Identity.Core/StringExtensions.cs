using FluentValidation;
using FluentValidation.Results;
using Logitar.EventSourcing;

namespace Logitar.Identity.Core;

/// <summary>
/// TODO(fpion): Get or Parse?
/// </summary>
public static class StringExtensions
{
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
}
