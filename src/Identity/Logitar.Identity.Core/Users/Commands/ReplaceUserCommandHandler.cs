using Logitar.EventSourcing;
using Logitar.Identity.Core.Roles.Queries;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using MediatR;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Core.Users.Commands;

public class ReplaceUserCommandHandler : IRequestHandler<ReplaceUserCommand, User?>
{
  private readonly IMediator _mediator;
  private readonly IUserQuerier _userQuerier;
  private readonly IUserRepository _userRepository;
  private readonly IOptions<UserSettings> _userSettings;

  public ReplaceUserCommandHandler(IMediator mediator, IUserQuerier userQuerier,
    IUserRepository userRepository, IOptions<UserSettings> userSettings)
  {
    _mediator = mediator;
    _userQuerier = userQuerier;
    _userRepository = userRepository;
    _userSettings = userSettings;
  }

  public async Task<User?> Handle(ReplaceUserCommand command, CancellationToken cancellationToken)
  {
    AggregateId userId = command.Id.GetAggregateId(nameof(command.Id));
    UserAggregate? user = await _userRepository.LoadAsync(userId, cancellationToken);
    if (user == null)
    {
      return null;
    }

    ReplaceUserPayload payload = command.Payload;
    UserSettings userSettings = _userSettings.Value;

    if (payload.UniqueName != null)
    {
      UserAggregate? other = await _userRepository.LoadAsync(user.TenantId, payload.UniqueName, cancellationToken);
      if (other?.Equals(user) == false)
      {
        throw new UniqueNameAlreadyUsedException<UserAggregate>(user.TenantId, payload.UniqueName, nameof(payload.UniqueName));
      }

      user.SetUniqueName(userSettings.UniqueNameSettings, payload.UniqueName);
    }
    if (payload.Password != null)
    {
      user.SetPassword(userSettings.PasswordSettings, payload.Password);
    }

    if (payload.IsDisabled != null)
    {
      if (payload.IsDisabled.Value)
      {
        user.Disable();
      }
      else
      {
        user.Enable();
      }
    }

    bool isAddressVerified = payload.Address?.IsVerified ?? user.Address?.IsVerified ?? false;
    PostalAddress? address = payload.Address == null ? null : new(payload.Address.Street,
      payload.Address.Locality, payload.Address.Country, payload.Address.Region,
      payload.Address.PostalCode, isAddressVerified);
    user.Address = address;

    bool isEmailVerified = payload.Email?.IsVerified ?? user.Email?.IsVerified ?? false;
    EmailAddress? email = payload.Email == null ? null : new(payload.Email.Address, isEmailVerified);
    if (email != null && userSettings.RequireUniqueEmail)
    {
      IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(user.TenantId, email, cancellationToken);
      if (users.Any(u => !u.Equals(user)))
      {
        throw new EmailAddressAlreadyUsedException(user.TenantId, email, nameof(payload.Email));
      }
    }
    user.Email = email;

    bool isPhoneVerified = payload.Phone?.IsVerified ?? user.Phone?.IsVerified ?? false;
    PhoneNumber? phone = payload.Phone == null ? null : new(payload.Phone.Number,
      payload.Phone.CountryCode, payload.Phone.Extension, isPhoneVerified);
    user.Phone = phone;

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

    Dictionary<AggregateId, RoleAggregate> roles = (await _mediator.Send(new FindRolesQuery(user.TenantId, payload.Roles, nameof(payload.Roles)), cancellationToken))
      .ToDictionary(x => x.Id, x => x);
    foreach (AggregateId roleId in user.Roles)
    {
      if (!roles.ContainsKey(roleId))
      {
        user.RemoveRole(roleId);
      }
    }
    foreach (RoleAggregate role in roles.Values)
    {
      user.AddRole(role);
    }

    await _userRepository.SaveAsync(user, cancellationToken);

    return await _userQuerier.ReadAsync(user, cancellationToken);
  }
}
