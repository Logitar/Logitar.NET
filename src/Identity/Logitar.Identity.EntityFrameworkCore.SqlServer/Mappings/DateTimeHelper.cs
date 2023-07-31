﻿namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Mappings;

public static class DateTimeHelper
{
  public static DateTime? ToUniversalTime(DateTime? dateTime)
    => dateTime.HasValue ? DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc) : null;
}