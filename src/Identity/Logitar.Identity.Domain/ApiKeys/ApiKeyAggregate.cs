using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.ApiKeys.Events;
using Logitar.Identity.Domain.ApiKeys.Validators;
using Logitar.Security;

namespace Logitar.Identity.Domain.ApiKeys;

public class ApiKeyAggregate : AggregateRoot
{
  public const int SecretLength = 256 / 8;

  private Pbkdf2 _secret = new(string.Empty);

  private string _title = string.Empty;
  private string? _description = null;
  private DateTime? _expiresOn = null;

  public ApiKeyAggregate(AggregateId id) : base(id)
  {
  }

  public ApiKeyAggregate(string title, string? tenantId = null) : base()
  {
    Secret = RandomNumberGenerator.GetBytes(SecretLength);

    ApiKeyCreatedEvent created = new()
    {
      Secret = new Pbkdf2(Secret),
      TenantId = tenantId?.CleanTrim(),
      Title = title.Trim()
    };

    new ApiKeyCreatedValidator().ValidateAndThrow(created);

    ApplyChange(created);
  }
  protected virtual void Apply(ApiKeyCreatedEvent created)
  {
    _secret = created.Secret;

    _title = created.Title;

    TenantId = created.TenantId;
  }

  public string? TenantId { get; private set; }

  public string Title
  {
    get => _title;
    set
    {
      value = value.Trim();
      new TitleValidator(nameof(Title)).ValidateAndThrow(value);

      if (value != _title)
      {
        ApiKeyUpdatedEvent updated = GetLatestUpdatedEvent();
        updated.Title = value;
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
        ApiKeyUpdatedEvent updated = GetLatestUpdatedEvent();
        updated.Description = new MayBe<string>(value);
        Apply(updated);
      }
    }
  }
  public DateTime? ExpiresOn
  {
    get => _expiresOn;
    set
    {
      if (value.HasValue)
      {
        new ExpiresOnValidator(nameof(ExpiresOn)).ValidateAndThrow(value.Value);
      }

      if ((value == null && _expiresOn.HasValue) || (value > _expiresOn))
      {
        throw new CannotPostponeApiKeyExpirationException(this, value, nameof(ExpiresOn));
      }

      if (value != _expiresOn)
      {
        ApiKeyUpdatedEvent updated = GetLatestUpdatedEvent();
        updated.ExpiresOn = value;
        Apply(updated);
      }
    }
  }

  public DateTime? AuthenticatedOn { get; private set; }

  public byte[]? Secret { get; private set; }

  public void Authenticate(byte[] secret)
  {
    if (!_secret.IsMatch(secret))
    {
      StringBuilder message = new();
      message.AppendLine("The specified secret does not match the API key.");
      message.Append("ApiKey: ").AppendLine(ToString());
      message.Append("Secret: ").AppendLine(Convert.ToBase64String(secret));
      throw new InvalidCredentialsException(message.ToString());
    }

    ApplyChange(new ApiKeyAuthenticatedEvent());
  }
  protected virtual void Apply(ApiKeyAuthenticatedEvent authenticated) => AuthenticatedOn = authenticated.OccurredOn;

  public void Delete() => ApplyChange(new ApiKeyDeletedEvent());

  protected virtual void Apply(ApiKeyUpdatedEvent updated)
  {
    if (updated.Title != null)
    {
      _title = updated.Title;
    }
    if (updated.Description != null)
    {
      _description = updated.Description.Value;
    }
    if (updated.ExpiresOn.HasValue)
    {
      _expiresOn = updated.ExpiresOn.Value;
    }
  }
  protected ApiKeyUpdatedEvent GetLatestUpdatedEvent()
  {
    ApiKeyUpdatedEvent? updated = Changes.LastOrDefault(e => e is ApiKeyUpdatedEvent) as ApiKeyUpdatedEvent;
    if (updated == null)
    {
      updated = new();
      ApplyChange(updated);
    }

    return updated;
  }

  public override string ToString() => $"{Title} | {base.ToString()}";
}
