using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Roles.Models;

namespace Logitar.Identity.Core.ApiKeys.Models;

/// <summary>
/// Represents the read model for API keys.
/// </summary>
public record ApiKey : Aggregate
{
  /// <summary>
  /// Gets or sets the identifier of the tenant in which the API key belongs.
  /// </summary>
  public string? TenantId { get; set; }

  /// <summary>
  /// Gets or sets the title of the API key.
  /// </summary>
  public string Title { get; set; } = string.Empty;
  /// <summary>
  /// Gets or sets the description of the API key.
  /// </summary>
  public string? Description { get; set; }
  /// <summary>
  /// Gets or sets the expiration date and time of the API key.
  /// </summary>
  public DateTime? ExpiresOn { get; set; }

  /// <summary>
  /// Gets or sets the last authentication date an time of the API key.
  /// </summary>
  public DateTime? AuthenticatedOn { get; set; }

  /// <summary>
  /// Gets or sets the custom attributes of the API key.
  /// </summary>
  public IEnumerable<CustomAttribute> CustomAttributes { get; set; } = Enumerable.Empty<CustomAttribute>();

  /// <summary>
  /// Gets or sets the roles of the API key.
  /// </summary>
  public IEnumerable<Role> Roles { get; set; } = Enumerable.Empty<Role>();
}
