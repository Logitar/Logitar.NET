namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Mappings;

public static class MappingHelper
{
  public static DateTime? ToUniversalTime(DateTime? dateTime)
    => dateTime.HasValue ? DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc) : null;
}
