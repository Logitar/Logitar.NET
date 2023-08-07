using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles.Events;
using Logitar.Identity.Domain.Roles.Validators;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Validators;

namespace Logitar.Identity.Domain.Roles;

public class RoleAggregate : AggregateRoot
{
  private readonly Dictionary<string, string> _customAttributes = new();
  private readonly CustomAttributeValidator _customAttributeValidator = new();
  private readonly DisplayNameValidator _displayNameValidator = new(nameof(DisplayName));

  private string? _displayName = null;
  private string? _description = null;

  public RoleAggregate(AggregateId id) : base(id)
  {
  }

  public RoleAggregate(IUniqueNameSettings uniqueNameSettings, string uniqueName, string? tenantId = null) : base()
  {
    RoleCreatedEvent created = new()
    {
      UniqueName = uniqueName.Trim(),
      TenantId = tenantId?.CleanTrim()
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
        _displayNameValidator.ValidateAndThrow(value);
      }

      if (value != _displayName)
      {
        RoleUpdatedEvent updated = GetLatestEvent<RoleUpdatedEvent>();
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
        RoleUpdatedEvent updated = GetLatestEvent<RoleUpdatedEvent>();
        updated.Description = new MayBe<string>(value);
        Apply(updated);
      }
    }
  }

  public IReadOnlyDictionary<string, string> CustomAttributes => _customAttributes;

  public void Delete() => ApplyChange(new RoleDeletedEvent());

  public void RemoveCustomAttribute(string key)
  {
    key = key.Trim();
    if (_customAttributes.ContainsKey(key))
    {
      RoleUpdatedEvent updated = GetLatestEvent<RoleUpdatedEvent>();
      updated.CustomAttributes[key] = null;
      Apply(updated);
    }
  }

  public void SetCustomAttribute(string key, string value)
  {
    key = key.Trim();
    value = value.Trim();
    _customAttributeValidator.ValidateAndThrow(key, value);

    if (!_customAttributes.TryGetValue(key, out string? existingValue) || value != existingValue)
    {
      RoleUpdatedEvent updated = GetLatestEvent<RoleUpdatedEvent>();
      updated.CustomAttributes[key] = value;
      Apply(updated);
    }
  }

  public void SetUniqueName(IUniqueNameSettings uniqueNameSettings, string uniqueName)
  {
    uniqueName = uniqueName.Trim();
    new UniqueNameValidator(uniqueNameSettings, nameof(UniqueName)).ValidateAndThrow(uniqueName);

    RoleUpdatedEvent updated = GetLatestEvent<RoleUpdatedEvent>();
    updated.UniqueName = uniqueName;
    Apply(updated);
  }

  protected virtual void Apply(RoleUpdatedEvent updated)
  {
    if (updated.DisplayName != null)
    {
      _displayName = updated.DisplayName.Value;
    }
    if (updated.Description != null)
    {
      _description = updated.Description.Value;
    }
  }
  protected virtual T GetLatestEvent<T>() where T : DomainEvent, new()
  {
    T? change = Changes.LastOrDefault(change => change is T) as T;
    if (change == null)
    {
      change = new();
      ApplyChange(change);
    }

    return change;
  }

  public override string ToString() => $"{DisplayName ?? UniqueName} | {base.ToString()}";
}
