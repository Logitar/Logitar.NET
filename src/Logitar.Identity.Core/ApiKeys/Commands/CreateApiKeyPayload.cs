using Logitar.Identity.Core.Models;

namespace Logitar.Identity.Core.ApiKeys.Commands;

/// <summary>
/// The API key creation data.
/// </summary>
public record CreateApiKeyPayload
{
  /// <summary>
  /// Gets or sets the identifier of the API key.
  /// </summary>
  public string? Id { get; set; }

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
  /// Gets or sets the custom attributes of the API key.
  /// </summary>
  public IEnumerable<CustomAttribute>? CustomAttributes { get; set; }

  /// <summary>
  /// Gets or sets the roles of the API key.
  /// <br />Each item in the list can either be the identifier or the unique name of a role.
  /// <br />Each specified role must reside into the same tenant as the API key.
  /// </summary>
  public IEnumerable<string>? Roles { get; set; }
}
