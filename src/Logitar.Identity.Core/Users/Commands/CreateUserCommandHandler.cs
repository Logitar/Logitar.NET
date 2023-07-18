using Logitar.EventSourcing;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Settings;
using Logitar.Identity.Core.Users.Contact;
using Logitar.Identity.Core.Users.Models;
using MediatR;

namespace Logitar.Identity.Core.Users.Commands;

/// <summary>
/// Represents the handler for instances of <see cref="CreateUserCommand"/>.
/// </summary>
internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
{
  /// <summary>
  /// The application context.
  /// </summary>
  private readonly IApplicationContext _applicationContext;
  /// <summary>
  /// The role repository.
  /// </summary>
  private readonly IRoleRepository _roleRepository;
  /// <summary>
  /// The user repository.
  /// </summary>
  private readonly IUserRepository _userRepository;

  /// <summary>
  /// Initializes a new instance of the <see cref="CreateUserCommandHandler"/> class.
  /// </summary>
  /// <param name="applicationContext">The application context.</param>
  /// <param name="roleRepository">The role repository.</param>
  /// <param name="userRepository">The user repository.</param>
  public CreateUserCommandHandler(IApplicationContext applicationContext,
    IRoleRepository roleRepository,
    IUserRepository userRepository)
  {
    _applicationContext = applicationContext;
    _roleRepository = roleRepository;
    _userRepository = userRepository;
  }

  /// <summary>
  /// Handles the specified request.
  /// </summary>
  /// <param name="request">The request to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The resulting read model.</returns>
  /// <exception cref="UniqueNameAlreadyUsedException{UserAggregate}">The user unique name is already used.</exception>
  /// <exception cref="EmailAddressAlreadyUsedException">The email address is already used.</exception>
  /// <exception cref="RolesNotFoundException">One or more roles could not be found.</exception>
  public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
  {
    CreateUserPayload payload = request.Payload;

    if (await _userRepository.LoadAsync(payload.TenantId, payload.UniqueName, cancellationToken) != null)
    {
      throw new UniqueNameAlreadyUsedException<UserAggregate>(payload.TenantId, payload.UniqueName, nameof(payload.UniqueName));
    }

    IUserSettings userSettings = _applicationContext.UserSettings;
    IUniqueNameSettings uniqueNameSettings = userSettings.UniqueNameSettings;
    IPasswordSettings passwordSettings = userSettings.PasswordSettings;

    AggregateId? id = payload.Id?.ParseAggregateId(nameof(payload.Id));
    UserAggregate user = new(uniqueNameSettings, payload.UniqueName, payload.TenantId, id);
    if (payload.Password != null)
    {
      user.ChangePassword(passwordSettings, payload.Password);
    }
    if (payload.IsDisabled)
    {
      user.Disable();
    }

    if (payload.Address != null)
    {
      ReadOnlyAddress address = new(payload.Address.Street, payload.Address.Locality,
        payload.Address.Country, payload.Address.Region, payload.Address.PostalCode,
        payload.Address.IsVerified ?? false);
      user.SetAddress(address);
    }
    if (payload.Email != null)
    {
      ReadOnlyEmail email = new(payload.Email.Address, payload.Email.IsVerified ?? false);
      if (userSettings.RequireUniqueEmail && (await _userRepository.LoadAsync(user.TenantId, email, cancellationToken)).Any())
      {
        throw new EmailAddressAlreadyUsedException(user.TenantId, email, nameof(payload.Email));
      }

      user.SetEmail(email);
    }
    if (payload.Phone != null)
    {
      ReadOnlyPhone phone = new(payload.Phone.Number, payload.Phone.CountryCode,
        payload.Phone.Extension, payload.Phone.IsVerified ?? false);
      user.SetPhone(phone);
    }

    user.FirstName = payload.FirstName;
    user.MiddleName = payload.MiddleName;
    user.LastName = payload.LastName;
    user.Nickname = payload.Nickname;
    user.Birthdate = payload.Birthdate;
    user.Gender = payload.Gender?.ParseGender(nameof(payload.Gender));
    user.Locale = payload.Locale?.ParseLocale(nameof(payload.Locale));
    user.TimeZone = payload.TimeZone?.ParseTimeZone(nameof(payload.TimeZone));
    user.Picture = payload.Picture?.ParseUri(nameof(payload.Picture));
    user.Profile = payload.Profile?.ParseUri(nameof(payload.Profile));
    user.Website = payload.Website?.ParseUri(nameof(payload.Website));

    if (payload.CustomAttributes?.Any() == true)
    {
      foreach (CustomAttribute customAttribute in payload.CustomAttributes)
      {
        user.SetCustomAttribute(customAttribute.Key, customAttribute.Value);
      }
    }

    if (payload.Roles?.Any() == true)
    {
      Dictionary<string, RoleAggregate> rolesById = new();
      Dictionary<string, RoleAggregate> rolesByUniqueName = new();
      IEnumerable<RoleAggregate> roles = await _roleRepository.LoadAsync(user.TenantId, cancellationToken);
      foreach (RoleAggregate role in roles)
      {
        rolesById[role.Id.Value] = role;
        rolesByUniqueName[role.UniqueName.ToUpper()] = role;
      }

      List<string> missingRoles = new();

      foreach (string idOrUniqueName in payload.Roles)
      {
        _ = rolesById.TryGetValue(idOrUniqueName, out RoleAggregate? role);
        if (role == null)
        {
          _ = rolesByUniqueName.TryGetValue(idOrUniqueName.ToUpper(), out role);
        }

        if (role == null)
        {
          missingRoles.Add(idOrUniqueName);
        }
        else
        {
          user.AddRole(role);
        }
      }

      if (missingRoles.Any())
      {
        throw new RolesNotFoundException(missingRoles, nameof(payload.Roles));
      }
    }

    await _userRepository.SaveAsync(user, cancellationToken);

    return await _userRepository.ReadAsync(user, cancellationToken);
  }
}
