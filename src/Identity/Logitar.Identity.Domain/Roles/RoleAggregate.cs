﻿using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles.Events;
using Logitar.Identity.Domain.Roles.Validators;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Validators;

namespace Logitar.Identity.Domain.Roles;

public class RoleAggregate : AggregateRoot
{
  private readonly Dictionary<string, string> _customAttributes = new();

  private string? _displayName = null;
  private string? _description = null;

  public RoleAggregate(AggregateId id) : base(id)
  {
  }

  public RoleAggregate(IUniqueNameSettings uniqueNameSettings, string uniqueName, string? tenantId = null) : base()
  {
    RoleCreatedEvent created = new()
    {
      TenantId = tenantId?.CleanTrim(),
      UniqueName = uniqueName.Trim()
    };

    new RoleCreatedValidator(uniqueNameSettings).ValidateAndThrow(created);

    ApplyChange(created);
  }
  protected virtual void Apply(RoleCreatedEvent created)
  {
    TenantId = created.TenantId;

    UniqueName = created.UniqueName;
  }

  public string? TenantId { get; private set; }

  public string UniqueName { get; private set; } = string.Empty;
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
        RoleUpdatedEvent updated = GetLatestUpdatedEvent();
        updated.DisplayName = new MayBe<string>(value);
        Apply(updated);
      }
    }
  }
  public string? Description
  {
    get => _description;
    set
    {
      value = value?.CleanTrim();

      if (value != _description)
      {
        RoleUpdatedEvent updated = GetLatestUpdatedEvent();
        updated.Description = new MayBe<string>(value);
        Apply(updated);
      }
    }
  }

  public IReadOnlyDictionary<string, string> CustomAttributes => _customAttributes.AsReadOnly();

  public void Delete() => ApplyChange(new RoleDeletedEvent());

  public void RemoveCustomAttribute(string key)
  {
    key = key.Trim();
    if (_customAttributes.ContainsKey(key))
    {
      RoleUpdatedEvent updated = GetLatestUpdatedEvent();
      updated.CustomAttributes[key] = null;
      Apply(updated);
    }
  }

  public void SetCustomAttribute(string key, string value)
  {
    key = key.Trim();
    value = value.Trim();
    new CustomAttributeValidator().ValidateAndThrow(key, value);

    if (!_customAttributes.TryGetValue(key, out string? existingValue) || value != existingValue)
    {
      RoleUpdatedEvent updated = GetLatestUpdatedEvent();
      updated.CustomAttributes[key] = value;
      Apply(updated);
    }
  }

  public void SetUniqueName(IUniqueNameSettings uniqueNameSettings, string uniqueName)
  {
    uniqueName = uniqueName.Trim();
    new UniqueNameValidator(uniqueNameSettings, nameof(UniqueName)).ValidateAndThrow(uniqueName);

    RoleUpdatedEvent updated = GetLatestUpdatedEvent();
    updated.UniqueName = uniqueName;
    Apply(updated);
  }

  protected virtual void Apply(RoleUpdatedEvent updated)
  {
    if (updated.UniqueName != null)
    {
      UniqueName = updated.UniqueName;
    }
    if (updated.DisplayName != null)
    {
      _displayName = updated.DisplayName.Value;
    }
    if (updated.Description != null)
    {
      _description = updated.Description.Value;
    }

    foreach (var (key, value) in updated.CustomAttributes)
    {
      if (value == null)
      {
        _customAttributes.Remove(key);
      }
      else
      {
        _customAttributes[key] = value;
      }
    }
  }
  protected RoleUpdatedEvent GetLatestUpdatedEvent()
  {
    RoleUpdatedEvent? updated = Changes.LastOrDefault(e => e is RoleUpdatedEvent) as RoleUpdatedEvent;
    if (updated == null)
    {
      updated = new();
      ApplyChange(updated);
    }

    return updated;
  }

  public override string ToString() => $"{DisplayName ?? UniqueName} | {base.ToString()}";
}
