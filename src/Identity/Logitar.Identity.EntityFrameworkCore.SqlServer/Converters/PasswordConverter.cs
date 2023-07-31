﻿using Logitar.Security;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Converters;

public class PasswordConverter : JsonConverter<Password?>
{
  public override Password? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? password = reader.GetString();
    if (password == null)
    {
      return null;
    }

    string kind = password.Split(Password.Separator).First();
    return kind switch
    {
      Pbkdf2.Prefix => Pbkdf2.Decode(password),
      _ => throw new NotSupportedException($"The password kind '{kind}' is not supported."),
    }; // TODO(fpion): refactor
  }

  public override void Write(Utf8JsonWriter writer, Password? password, JsonSerializerOptions options)
  {
    writer.WriteStringValue(password?.Encode());
  }
}
