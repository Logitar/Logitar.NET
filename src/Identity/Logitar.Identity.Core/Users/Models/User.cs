using Logitar.Identity.Core.Models;

namespace Logitar.Identity.Core.Users.Models;

public record User : Aggregate
{
  public string? TenantId { get; set; }

  public string UniqueName { get; set; } = string.Empty;

  public bool HasPassword { get; set; }
  public Actor? PasswordChangedBy { get; set; }
  public DateTime? PasswordChangedOn { get; set; }

  public Actor? DisabledBy { get; set; }
  public DateTime? DisabledOn { get; set; }
  public bool IsDisabled { get; set; }

  public Email? Email { get; set; }

  public bool IsConfirmed { get; set; }

  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public string? FullName { get; set; }

  public string? Locale { get; set; }
}
