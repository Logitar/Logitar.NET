using Logitar.Identity.Domain.Users;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Logitar.Identity.Infrastructure;

public class TimeZoneEntryConverter : JsonConverter<TimeZoneEntry?>
{
  public override TimeZoneEntry? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();

    return value == null ? null : new TimeZoneEntry(value);
  }

  public override void Write(Utf8JsonWriter writer, TimeZoneEntry? timeZoneEntry, JsonSerializerOptions options)
  {
    writer.WriteStringValue(timeZoneEntry?.Id);
  }
}
