namespace Logitar.Identity.EntityFrameworkCore.SqlServer;

public interface ICustomAttributesProvider
{
  string? CustomAttributes { get; }
}
