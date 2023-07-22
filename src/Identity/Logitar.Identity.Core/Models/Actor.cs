namespace Logitar.Identity.Core.Models;

public record Actor
{
  public string Id { get; set; } = "SYSTEM";
  public string Type { get; set; } = "System";
  public bool IsDeleted { get; set; }

  public string DisplayName { get; set; } = "System";
  public string? EmailAddress { get; set; }
  public string? PictureUrl { get; set; }
}
