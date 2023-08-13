using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Infrastructure;

public class GenderConverter : JsonConverter<Gender?>
{
  public override Gender? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();

    return value == null ? null : new Gender(value);
  }

  public override void Write(Utf8JsonWriter writer, Gender? gender, JsonSerializerOptions options)
  {
    writer.WriteStringValue(gender?.Value);
  }
}
