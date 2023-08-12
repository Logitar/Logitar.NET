﻿using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Logitar.Identity.Infrastructure;

public class CultureInfoConverter : JsonConverter<CultureInfo?>
{
  public override CultureInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? name = reader.GetString();

    return name == null ? null : CultureInfo.GetCultureInfo(name);
  }

  public override void Write(Utf8JsonWriter writer, CultureInfo? culture, JsonSerializerOptions options)
  {
    writer.WriteStringValue(culture?.Name);
  }
}