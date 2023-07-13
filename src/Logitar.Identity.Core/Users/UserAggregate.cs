using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Core.Settings;
using Logitar.Identity.Core.Users.Events;
using Logitar.Identity.Core.Users.Validators;
using Logitar.Identity.Core.Validators;
using Logitar.Security.Cryptography;

namespace Logitar.Identity.Core.Users;

/// <summary>
/// TODO
/// </summary>
public class UserAggregate : AggregateRoot
{
  /// <summary>
  /// The password of the user.
  /// </summary>
  private Pbkdf2? _password = null;

  /// <summary>
  /// Initializes a new instance of the <see cref="UserAggregate"/> class.
  /// </summary>
  /// <param name="id">The identifier of the user.</param>
  public UserAggregate(AggregateId id) : base(id)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserAggregate"/> class.
  /// </summary>
  /// <param name="uniqueNameSettings">The settings used to validate the unique name.</param>
  /// <param name="uniqueName">The unique name of the user.</param>
  /// <param name="tenantId">The identifier of the tenant in which the user belongs.</param>
  /// <param name="id">The identifier of the user.</param>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public UserAggregate(IUniqueNameSettings uniqueNameSettings, string uniqueName,
    string? tenantId = null, AggregateId? id = null) : base(id)
  {
    UserCreatedEvent e = new()
    {
      TenantId = tenantId?.CleanTrim(),
      UniqueName = uniqueName.Trim()
    };

    UniqueNameValidator uniqueNameValidator = new(uniqueNameSettings);
    new UserCreatedValidator(uniqueNameValidator).ValidateAndThrow(e);

    ApplyChange(e);
  }
  /// <summary>
  /// Applies the specified event to the aggregate.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  protected virtual void Apply(UserCreatedEvent e)
  {
    TenantId = e.TenantId;

    UniqueName = e.UniqueName;
  }

  /// <summary>
  /// Gets or sets the identifier of the tenant in which the user belongs.
  /// </summary>
  public string? TenantId { get; private set; }

  /// <summary>
  /// Gets or sets the unique name of the user.
  /// </summary>
  public string UniqueName { get; private set; } = string.Empty;

  /// <summary>
  /// Gets a value indicating whether or not the user has a password.
  /// </summary>
  public bool HasPassword => _password != null;

  /// <summary>
  /// Changes the password of the user.
  /// </summary>
  /// <param name="passwordSettings">The settings used to validate the password.</param>
  /// <param name="password">The new password of the user.</param>
  /// <param name="current">The current password of the user, used to validate a password change from an user.</param>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public void ChangePassword(IPasswordSettings passwordSettings, string password, string? current = null)
  {
    if (current != null && _password?.IsMatch(current) != true)
    {
      StringBuilder message = new();
      message.AppendLine("The user password was not a match.");
      message.Append("User: ").Append(this).AppendLine();
      message.Append("Password: ").Append(password).AppendLine();
      throw new InvalidCredentialsException(message.ToString());
    }

    new PasswordValidator(passwordSettings).ValidateAndThrow(password);

    ApplyChange(new UserPasswordChangedEvent
    {
      Password = new(password)
    });
  }
  /// <summary>
  /// Applies the specified event to the aggregate.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  protected virtual void Apply(UserPasswordChangedEvent e) => _password = e.Password;

  /// <summary>
  /// Deletes the role.
  /// </summary>
  public void Delete() => ApplyChange(new UserDeletedEvent());

  /// <summary>
  /// Changes the unique name of the user.
  /// </summary>
  /// <param name="uniqueNameSettings">The settings used to validate the unique name.</param>
  /// <param name="uniqueName">The unique name of the user.</param>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public void SetUniqueName(IUniqueNameSettings uniqueNameSettings, string uniqueName)
  {
    uniqueName = uniqueName.Trim();
    new UniqueNameValidator(uniqueNameSettings, nameof(UniqueName)).ValidateAndThrow(uniqueName);

    if (uniqueName != UniqueName)
    {
      ApplyChange(new UserUniqueNameChangedEvent
      {
        UniqueName = uniqueName
      });
    }
  }
  /// <summary>
  /// Applies the specified event to the aggregate.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  protected virtual void Apply(UserUniqueNameChangedEvent e) => UniqueName = e.UniqueName;
}
