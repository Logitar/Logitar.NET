using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Core.ApiKeys.Events;
using Logitar.Identity.Core.ApiKeys.Validators;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Validators;
using Logitar.Security.Cryptography;

namespace Logitar.Identity.Core.ApiKeys;

/// <summary>
/// Represents an API key or token in the identity system. API keys/tokens are typically used in
/// backend-to-backend communications to avoid exposing user credentials.
/// </summary>
public class ApiKeyAggregate : AggregateRoot
{
  /// <summary>
  /// The length of API key secrets (256 bits).
  /// </summary>
  private const int SecretLength = 256 / 8;

  /// <summary>
  /// The custom attributes of the API key.
  /// </summary>
  private readonly Dictionary<string, string> _customAttributes = new();
  /// <summary>
  /// The roles of the API key.
  /// </summary>
  private readonly HashSet<AggregateId> _roles = new();

  /// <summary>
  /// The secret of the API key.
  /// </summary>
  private Pbkdf2 _secret = new(string.Empty);

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
    Secret = RandomNumberGenerator.GetBytes(SecretLength);

    ApiKeyCreatedEvent e = new()
    {
      TenantId = tenantId?.CleanTrim(),
      Secret = new Pbkdf2(Secret),
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

    _secret = e.Secret;

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
  /// Gets or sets the last authentication date an time of the API key.
  /// </summary>
  public DateTime? AuthenticatedOn { get; private set; }

  /// <summary>
  /// Gets the custom attributes of the API key.
  /// </summary>
  public IReadOnlyDictionary<string, string> CustomAttributes => _customAttributes.AsReadOnly();
  /// <summary>
  /// Gets the roles of the API key.
  /// </summary>
  public IReadOnlySet<AggregateId> Roles => _roles.ToImmutableHashSet();

  /// <summary>
  /// Gets or sets the secret bytes of the API key.
  /// </summary>
  public byte[]? Secret { get; private set; }

  /// <summary>
  /// Deletes the API key.
  /// </summary>
  public void Delete() => ApplyChange(new ApiKeyDeletedEvent());

  /// <summary>
  /// Adds the specified role to the API key.
  /// </summary>
  /// <param name="role">The role to add.</param>
  public void AddRole(RoleAggregate role)
  {
    if (!_roles.Contains(role.Id))
    {
      SetRole(role, addOrRemove: true);
    }
  }
  /// <summary>
  /// Removes the specified role from the API key.
  /// </summary>
  /// <param name="role">The role to remove.</param>
  public void RemoveRole(RoleAggregate role)
  {
    if (_roles.Contains(role.Id))
    {
      SetRole(role, addOrRemove: false);
    }
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
  /// Authenticates the API key.
  /// </summary>
  /// <param name="secret">The secret to match against.</param>
  /// <param name="moment">The expiration date and time to validate against.</param>
  /// <exception cref="NotImplementedException"></exception>
  public void Authenticate(byte[] secret, DateTime? moment = null)
  {
    if (!IsMatch(secret))
    {
      StringBuilder message = new();
      message.AppendLine("The API key secret was not a match.");
      message.Append("ApiKey: ").Append(this).AppendLine();
      message.Append("Secret: ").Append(Convert.ToBase64String(secret)).AppendLine();
      throw new InvalidCredentialsException(message.ToString());
    }
    else if (IsExpired(moment))
    {
      throw new ApiKeyIsExpiredException(this, moment);
    }

    ApplyChange(new ApiKeyAuthenticatedEvent(), occurredOn: moment);
  }
  /// <summary>
  /// Applies the specified event to the aggregate.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  protected virtual void Apply(ApiKeyAuthenticatedEvent e) => AuthenticatedOn = e.OccurredOn;

  /// <summary>
  /// Returns a value indicating whether or not the API key is expired.
  /// </summary>
  /// <param name="moment">The date and time to validate against. Defaults to now.</param>
  /// <returns>The expiration status.</returns>
  public bool IsExpired(DateTime? moment = null) => _expiresOn <= (moment ?? DateTime.Now);

  /// <summary>
  /// Returns a value indicating whether or not the specified secret is a match to the API key.
  /// </summary>
  /// <param name="secret">The secret to match against.</param>
  /// <returns>The match result.</returns>
  public bool IsMatch(byte[] secret) => _secret.IsMatch(secret);

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
