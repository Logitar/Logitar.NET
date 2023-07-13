﻿using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Core.ApiKeys.Events;
using Logitar.Identity.Core.ApiKeys.Validators;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Roles.Validators;
using Logitar.Identity.Core.Validators;
using System.Collections.Immutable;

namespace Logitar.Identity.Core.ApiKeys;

/// <summary>
/// Represents an API key or token in the identity system. API keys/tokens are typically used in
/// backend-to-backend communications to avoid exposing user credentials.
/// </summary>
public class ApiKeyAggregate : AggregateRoot
{
  /// <summary>
  /// The custom attributes of the API key.
  /// </summary>
  private readonly Dictionary<string, string> _customAttributes = new();
  /// <summary>
  /// The roles of the API key.
  /// </summary>
  private readonly HashSet<AggregateId> _roles = new();

  /// <summary>
  /// The title of the API key.
  /// </summary>
  private string _title = string.Empty;
  /// <summary>
  /// The description of the API key.
  /// </summary>
  private string? _description = null;
  /// <summary>
  /// The expiration date and time of the API key.
  /// </summary>
  private DateTime? _expiresOn = null;

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyAggregate"/> class.
  /// </summary>
  /// <param name="id">The identifier of the API key.</param>
  public ApiKeyAggregate(AggregateId id) : base(id)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyAggregate"/> class.
  /// </summary>
  /// <param name="title">The title of the API key.</param>
  /// <param name="tenantId">The identifier of the tenant in which the API key belongs.</param>
  /// <param name="id">The identifier of the API key.</param>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public ApiKeyAggregate(string title, string? tenantId = null, AggregateId? id = null) : base(id)
  {
    ApiKeyCreatedEvent e = new()
    {
      TenantId = tenantId?.CleanTrim(),
      Title = title.Trim()
    };

    TitleValidator titleValidator = new();
    new ApiKeyCreatedValidator(titleValidator).ValidateAndThrow(e);

    ApplyChange(e);
  }
  /// <summary>
  /// Applies the specified event to the aggregate.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  protected virtual void Apply(ApiKeyCreatedEvent e)
  {
    TenantId = e.TenantId;

    _title = e.Title;
  }

  /// <summary>
  /// Gets or sets the identifier of the tenant in which the API key belongs.
  /// </summary>
  public string? TenantId { get; private set; }

  /// <summary>
  /// Gets or sets the title of the API key.
  /// </summary>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public string Title
  {
    get => _title;
    set
    {
      value = value.Trim();
      new TitleValidator(nameof(Title)).ValidateAndThrow(value);

      if (value != _title)
      {
        ApiKeyModifiedEvent e = GetLatestModifiedEvent();
        e.Title = new Modification<string>(value);
        Apply(e);
      }
    }
  }
  /// <summary>
  /// Gets or sets the description of the API key.
  /// </summary>
  public string? Description
  {
    get => _description;
    set
    {
      value = value?.CleanTrim();
      if (value != _description)
      {
        ApiKeyModifiedEvent e = GetLatestModifiedEvent();
        e.Description = new Modification<string>(value);
        Apply(e);
      }
    }
  }
  /// <summary>
  /// Gets or sets the expiration date and time of the API key.
  /// </summary>
  /// <exception cref="CannotPostponeApiKeyExpirationException">The input expiration would postpone the expiration of the API key.</exception>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public DateTime? ExpiresOn
  {
    get => _expiresOn;
    set
    {
      if ((!value.HasValue && _expiresOn.HasValue) || value > _expiresOn)
      {
        throw new CannotPostponeApiKeyExpirationException(this, value);
      }

      if (value.HasValue)
      {
        new ExpiresOnValidator(nameof(ExpiresOn)).ValidateAndThrow(value.Value);
      }

      if (value != _expiresOn)
      {
        ApiKeyModifiedEvent e = GetLatestModifiedEvent();
        e.ExpiresOn = new Modification<DateTime?>(value);
        Apply(e);
      }
    }
  }

  /// <summary>
  /// Gets the custom attributes of the API key.
  /// </summary>
  public IReadOnlyDictionary<string, string> CustomAttributes => _customAttributes.AsReadOnly();
  /// <summary>
  /// Gets the roles of the API key.
  /// </summary>
  public IReadOnlySet<AggregateId> Roles => _roles.ToImmutableHashSet();

  /// <summary>
  /// Deletes the API key.
  /// </summary>
  public void Delete() => ApplyChange(new ApiKeyDeletedEvent());

  /// <summary>
  /// Adds the specified role to the API key.
  /// </summary>
  /// <param name="role">The role to add.</param>
  /// <returns>A value indicating whether or not the role was added.</returns>
  public bool AddRole(RoleAggregate role)
  {
    if (_roles.Contains(role.Id))
    {
      return false;
    }

    SetRole(role, addOrRemove: true);

    return true;
  }
  /// <summary>
  /// Removes the specified role from the API key.
  /// </summary>
  /// <param name="role">The role to remove.</param>
  /// <returns>A value indicating whether or not the role was removed.</returns>
  public bool RemoveRole(RoleAggregate role)
  {
    if (!_roles.Contains(role.Id))
    {
      return false;
    }

    SetRole(role, addOrRemove: false);

    return true;
  }
  /// <summary>
  /// Sets the status of the specified role.
  /// </summary>
  /// <param name="role">The role to add or remove.</param>
  /// <param name="addOrRemove">A value indicating whether the role will be added if true or removed if false.</param>
  private void SetRole(RoleAggregate role, bool addOrRemove)
  {
    ApiKeyModifiedEvent e = GetLatestModifiedEvent();
    e.Roles[role.Id.Value] = addOrRemove;
    Apply(e);
  }

  /// <summary>
  /// Removes a custom attribute on the API key.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  public void RemoveCustomAttribute(string key)
  {
    key = key.Trim();
    if (_customAttributes.ContainsKey(key))
    {
      ApiKeyModifiedEvent e = GetLatestModifiedEvent();
      e.CustomAttributes[key] = null;
      Apply(e);
    }
  }
  /// <summary>
  /// Sets a custom attribute on the API key.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  /// <param name="value">The value of the custom attribute.</param>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public void SetCustomAttribute(string key, string value)
  {
    key = key.Trim();
    value = value.Trim();
    CustomAttributeValidator.Instance.ValidateAndThrow(key, value);

    if (!_customAttributes.TryGetValue(key, out string? existingValue) || value != existingValue)
    {
      ApiKeyModifiedEvent e = GetLatestModifiedEvent();
      e.CustomAttributes[key] = value;
      Apply(e);
    }
  }

  /// <summary>
  /// Returns a value indicating whether or not the API key is expired.
  /// </summary>
  /// <param name="moment">The date and time to validate against. Defaults to now.</param>
  /// <returns>The expiration status.</returns>
  public bool IsExpired(DateTime? moment = null) => _expiresOn >= (moment ?? DateTime.Now);

  /// <summary>
  /// Applies the specified event to the aggregate.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  protected virtual void Apply(ApiKeyModifiedEvent e)
  {
    if (e.Title.IsModified && e.Title.Value != null)
    {
      _title = e.Title.Value;
    }
    if (e.Description.IsModified)
    {
      _description = e.Description.Value;
    }
    if (e.ExpiresOn.IsModified)
    {
      _expiresOn = e.ExpiresOn.Value;
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

    foreach (KeyValuePair<string, bool> role in e.Roles)
    {
      AggregateId roleId = new(role.Key);

      if (role.Value)
      {
        _roles.Add(roleId);
      }
      else
      {
        _roles.Remove(roleId);
      }
    }
  }
  /// <summary>
  /// Finds or applies the latest API key modification event.
  /// </summary>
  /// <returns>The latest API key modification event.</returns>
  private ApiKeyModifiedEvent GetLatestModifiedEvent()
  {
    ApiKeyModifiedEvent? e = Changes.LastOrDefault(e => e is ApiKeyModifiedEvent) as ApiKeyModifiedEvent;
    if (e == null)
    {
      e = new();
      ApplyChange(e);
    }

    return e;
  }

  /// <summary>
  /// Returns a string representation of the API key aggregate.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => string.Join(" | ", Title, base.ToString());
}
