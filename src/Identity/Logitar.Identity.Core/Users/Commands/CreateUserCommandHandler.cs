using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Passwords;
using Logitar.Identity.Core.Roles.Queries;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using Logitar.Security.Cryptography;
using MediatR;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Core.Users.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
{
  private readonly IMediator _mediator;
  private readonly IPasswordHelper _passwordHelper;
  private readonly IUserQuerier _userQuerier;
  private readonly IUserRepository _userRepository;
  private readonly IOptions<UserSettings> _userSettings;

  public CreateUserCommandHandler(IMediator mediator, IPasswordHelper passwordHelper,
    IUserQuerier userQuerier, IUserRepository userRepository, IOptions<UserSettings> userSettings)
  {
    _mediator = mediator;
    _passwordHelper = passwordHelper;
    _userQuerier = userQuerier;
    _userRepository = userRepository;
    _userSettings = userSettings;
  }

  public async Task<User> Handle(CreateUserCommand command, CancellationToken cancellationToken)
  {
    CreateUserPayload payload = command.Payload;

    if (await _userRepository.LoadAsync(payload.TenantId, payload.UniqueName, cancellationToken) != null)
    {
      throw new UniqueNameAlreadyUsedException<UserAggregate>(payload.TenantId, payload.UniqueName, nameof(payload.UniqueName));
    }

    UserSettings userSettings = _userSettings.Value;

    UserAggregate user = new(userSettings.UniqueNameSettings, payload.UniqueName, payload.TenantId);

    if (payload.Password != null)
    {
      Password password = _passwordHelper.Create(payload.Password);
      user.SetPassword(password);
    }

    if (payload.IsDisabled)
    {
      user.Disable();
    }

    if (payload.Address != null)
    {
      PostalAddress address = new(payload.Address.Street, payload.Address.Locality,
        payload.Address.Country, payload.Address.Region, payload.Address.PostalCode,
        payload.Address.IsVerified);
      user.Address = address;
    }
    if (payload.Email != null)
    {
      EmailAddress email = new(payload.Email.Address, payload.Email.IsVerified);
      if (userSettings.RequireUniqueEmail
        && (await _userRepository.LoadAsync(user.TenantId, email, cancellationToken)).Any())
      {
        throw new EmailAddressAlreadyUsedException(user.TenantId, email, nameof(payload.Email));
      }

      user.Email = email;
    }
    if (payload.Phone != null)
    {
      PhoneNumber phone = new(payload.Phone.Number, payload.Phone.CountryCode,
        payload.Phone.Extension, payload.Phone.IsVerified);
      user.Phone = phone;
    }

    user.FirstName = payload.FirstName;
    user.MiddleName = payload.MiddleName;
    user.LastName = payload.LastName;
    user.Nickname = payload.Nickname;

    user.Birthdate = payload.Birthdate;
    user.Gender = payload.Gender?.GetGender(nameof(payload.Gender));
    user.Locale = payload.Locale?.GetCultureInfo(nameof(payload.Locale));
    user.TimeZone = payload.TimeZone?.GetTimeZone(nameof(payload.TimeZone));

    user.Picture = payload.Picture?.GetUri(nameof(payload.Picture));
    user.Profile = payload.Profile?.GetUri(nameof(payload.Profile));
    user.Website = payload.Website?.GetUri(nameof(payload.Website));

    foreach (CustomAttribute customAttribute in payload.CustomAttributes)
    {
      user.SetCustomAttribute(customAttribute.Key, customAttribute.Value);
    }

    IEnumerable<RoleAggregate> roles = await _mediator.Send(new FindRolesQuery(user.TenantId, payload.Roles, nameof(payload.Roles)), cancellationToken);
    foreach (RoleAggregate role in roles)
    {
      user.AddRole(role);
    }

    await _userRepository.SaveAsync(user, cancellationToken);

    return await _userQuerier.ReadAsync(user, cancellationToken);
  }
}
