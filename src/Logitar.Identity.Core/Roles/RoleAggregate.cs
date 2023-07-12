using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Core.Roles.Events;
using Logitar.Identity.Core.Roles.Validators;
using Logitar.Identity.Core.Settings;
using Logitar.Identity.Core.Validators;

namespace Logitar.Identity.Core.Roles;

/// <summary>
/// Represents a role in the identity system. Roles are typically assigned to actors, such as users
/// and API keys, to control permissions in authorization systems.
/// </summary>
public class RoleAggregate : AggregateRoot
{
  /// <summary>
  /// The custom attributes of the role.
  /// </summary>
  private readonly Dictionary<string, string> _customAttributes = new();

  /// <summary>
  /// The display name of the role.
  /// </summary>
  private string? _displayName = null;
  /// <summary>
  /// The description of the role.
  /// </summary>
  private string? _description = null;

  /// <summary>
  /// Initializes a new instance of the <see cref="RoleAggregate"/> class.
  /// </summary>
  /// <param name="id">The identifier of the role.</param>
  public RoleAggregate(AggregateId id) : base(id)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="RoleAggregate"/> class.
  /// </summary>
  /// <param name="uniqueNameSettings">The settings used to validate the unique name.</param>
  /// <param name="uniqueName">The unique name of the role.</param>
  /// <param name="tenantId">The identifier of the tenant in which the role belongs.</param>
  /// <param name="id">The identifier of the role.</param>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public RoleAggregate(IUniqueNameSettings uniqueNameSettings, string uniqueName,
    string? tenantId = null, AggregateId? id = null) : base(id)
  {
    RoleCreatedEvent e = new()
    {
      TenantId = tenantId?.CleanTrim(),
      UniqueName = uniqueName.Trim()
    };

    UniqueNameValidator uniqueNameValidator = new(uniqueNameSettings);
    new RoleCreatedValidator(uniqueNameValidator).ValidateAndThrow(e);

    ApplyChange(e);
  }
  /// <summary>
  /// Applies the specified event to the aggregate.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  protected virtual void Apply(RoleCreatedEvent e)
  {
    TenantId = e.TenantId;

    UniqueName = e.UniqueName;
  }

  /// <summary>
  /// Gets or sets the identifier of the tenant in which the role belongs.
  /// </summary>
  public string? TenantId { get; private set; }

  /// <summary>
  /// Gets or sets the unique name of the role.
  /// </summary>
  public string UniqueName { get; private set; } = string.Empty;
  /// <summary>
  /// Gets or sets the display name of the role.
  /// </summary>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public string? DisplayName
  {
    get => _displayName;
    set
    {
      value = value?.CleanTrim();
      if (value != null)
      {
        new DisplayNameValidator(nameof(DisplayName)).ValidateAndThrow(value);
      }

      if (value != _displayName)
      {
        RoleModifiedEvent e = GetLatestModifiedEvent();
        e.DisplayName = new Modification<string>(value);
        Apply(e);
      }
    }
  }
  /// <summary>
  /// Gets or sets the description of the role.
  /// </summary>
  public string? Description
  {
    get => _description;
    set
    {
      value = value?.CleanTrim();
      if (value != _description)
      {
        RoleModifiedEvent e = GetLatestModifiedEvent();
        e.Description = new Modification<string>(value);
        Apply(e);
      }
    }
  }

  /// <summary>
  /// Gets the custom attributes of the role.
  /// </summary>
  public IReadOnlyDictionary<string, string> CustomAttributes => _customAttributes.AsReadOnly();

  /// <summary>
  /// Deletes the aggregate.
  /// </summary>
  public void Delete() => ApplyChange(new RoleDeletedEvent());

  /// <summary>
  /// Removes a custom attribute on the role.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  public void RemoveCustomAttribute(string key)
  {
    key = key.Trim();
    if (_customAttributes.ContainsKey(key))
    {
      RoleModifiedEvent e = GetLatestModifiedEvent();
      e.CustomAttributes[key] = null;
      Apply(e);
    }
  }
  /// <summary>
  /// Sets a custom attribute on the role.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  /// <param name="value">The value of the custom attribute.</param>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public void SetCustomAttribute(string key, string value)
  {
    key = key.Trim();
    value = value.Trim();
    CustomAttributeValidator.Instance.ValidateAndThrow(key, value);

    RoleModifiedEvent e = GetLatestModifiedEvent();
    e.CustomAttributes[key] = value;
    Apply(e);
  }

  /// <summary>
  /// Applies the specified event to the aggregate.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  protected virtual void Apply(RoleModifiedEvent e)
  {
    if (e.DisplayName.IsModified)
    {
      _displayName = e.DisplayName.Value;
    }
    if (e.Description.IsModified)
    {
      _description = e.Description.Value;
    }

    foreach (KeyValuePair<string, string?> customAttribute in e.CustomAttributes)
    {
      if (customAttribute.Value == null)
      {
        _customAttributes.Remove(customAttribute.Key);
      }
      else
      {
        _customAttributes[customAttribute.Key] = customAttribute.Value;
      }
    }
  }
  /// <summary>
  /// Finds or applies the latest role modification event.
  /// </summary>
  /// <returns>The latest role modification event.</returns>
  private RoleModifiedEvent GetLatestModifiedEvent()
  {
    RoleModifiedEvent? e = Changes.LastOrDefault(e => e is RoleModifiedEvent) as RoleModifiedEvent;
    if (e == null)
    {
      e = new();
      ApplyChange(e);
    }

    return e;
  }

  /// <summary>
  /// Returns a string representation of the role aggregate.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => string.Join(" | ", DisplayName ?? UniqueName, base.ToString());
}
