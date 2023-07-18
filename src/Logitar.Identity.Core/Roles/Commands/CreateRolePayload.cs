using Logitar.Identity.Core.Models;

namespace Logitar.Identity.Core.Roles.Commands;

/// <summary>
/// The role creation data.
/// </summary>
public record CreateRolePayload
{
  /// <summary>
  /// Gets or sets the identifier of the role.
  /// </summary>
  public string? Id { get; set; }

  /// <summary>
  /// Gets or sets the identifier of the tenant in which the role belongs.
  /// </summary>
  public string? TenantId { get; set; }

  /// <summary>
  /// Gets or sets the unique name of the role.
  /// </summary>
  public string UniqueName { get; set; } = string.Empty;
  /// <summary>
  /// Gets or sets the display name of the role.
  /// </summary>
  public string? DisplayName { get; set; }
  /// <summary>
  /// Gets or sets the description of the role.
  /// </summary>
  public string? Description { get; set; }

  /// <summary>
  /// Gets or sets the custom attributes of the role.
  /// </summary>
  public IEnumerable<CustomAttribute>? CustomAttributes { get; set; }
}
