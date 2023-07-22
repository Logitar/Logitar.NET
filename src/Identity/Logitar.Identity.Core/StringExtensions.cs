using FluentValidation;
using FluentValidation.Results;

namespace Logitar.Identity.Core;

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
}
