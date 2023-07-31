using Logitar.EventSourcing;
using Logitar.Identity.Core.Passwords;
using Logitar.Identity.Core.Payloads;
using Logitar.Identity.Core.Roles.Queries;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using Logitar.Security.Cryptography;
using MediatR;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Core.Users.Commands;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, User?>
{
  private readonly IMediator _mediator;
  private readonly IPasswordHelper _passwordHelper;
  private readonly IUserQuerier _userQuerier;
  private readonly IUserRepository _userRepository;
  private readonly IOptions<UserSettings> _userSettings;

  public UpdateUserCommandHandler(IMediator mediator, IPasswordHelper passwordHelper,
    IUserQuerier userQuerier, IUserRepository userRepository, IOptions<UserSettings> userSettings)
  {
    _mediator = mediator;
    _passwordHelper = passwordHelper;
    _userQuerier = userQuerier;
    _userRepository = userRepository;
    _userSettings = userSettings;
  }

  public async Task<User?> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
  {
    AggregateId userId = command.Id.GetAggregateId(nameof(command.Id));
    UserAggregate? user = await _userRepository.LoadAsync(userId, cancellationToken);
    if (user == null)
    {
      return null;
    }

    UpdateUserPayload payload = command.Payload;
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
      Password password = _passwordHelper.Create(payload.Password);
      user.SetPassword(password);
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

    if (payload.Address != null)
    {
      bool isVerified = payload.Address.Value?.IsVerified ?? user.Address?.IsVerified ?? false;
      PostalAddress? address = payload.Address.Value == null ? null
        : new(payload.Address.Value.Street, payload.Address.Value.Locality,
          payload.Address.Value.Country, payload.Address.Value.Region,
          payload.Address.Value.PostalCode, isVerified);
      user.Address = address;
    }
    if (payload.Email != null)
    {
      bool isVerified = payload.Email.Value?.IsVerified ?? user.Email?.IsVerified ?? false;
      EmailAddress? email = payload.Email.Value == null ? null
        : new(payload.Email.Value.Address, isVerified);
      if (email != null)
      {
        IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(user.TenantId, email, cancellationToken);
        if (users.Any(u => !u.Equals(user)))
        {
          throw new EmailAddressAlreadyUsedException(user.TenantId, email, nameof(payload.Email));
        }
      }

      user.Email = email;
    }
    if (payload.Phone != null)
    {
      bool isVerified = payload.Phone.Value?.IsVerified ?? user.Phone?.IsVerified ?? false;
      PhoneNumber? phone = payload.Phone.Value == null ? null
        : new(payload.Phone.Value.Number, payload.Phone.Value.CountryCode,
          payload.Phone.Value.Extension, isVerified);
      user.Phone = phone;
    }

    if (payload.FirstName != null)
    {
      user.FirstName = payload.FirstName.Value;
    }
    if (payload.MiddleName != null)
    {
      user.MiddleName = payload.MiddleName.Value;
    }
    if (payload.LastName != null)
    {
      user.LastName = payload.LastName.Value;
    }
    if (payload.Nickname != null)
    {
      user.Nickname = payload.Nickname.Value;
    }

    if (payload.Birthdate != null)
    {
      user.Birthdate = payload.Birthdate.Value;
    }
    if (payload.Gender != null)
    {
      user.Gender = payload.Gender.Value?.GetGender(nameof(payload.Gender));
    }
    if (payload.Locale != null)
    {
      user.Locale = payload.Locale.Value?.GetCultureInfo(nameof(payload.Locale));
    }
    if (payload.TimeZone != null)
    {
      user.TimeZone = payload.TimeZone.Value?.GetTimeZone(nameof(payload.TimeZone));
    }

    if (payload.Picture != null)
    {
      user.Picture = payload.Picture.Value?.GetUri(nameof(payload.Picture));
    }
    if (payload.Profile != null)
    {
      user.Profile = payload.Profile.Value?.GetUri(nameof(payload.Profile));
    }
    if (payload.Website != null)
    {
      user.Website = payload.Website.Value?.GetUri(nameof(payload.Website));
    }

    IEnumerable<string> roleIds = payload.Roles.Select(role => role.Role);
    IEnumerable<RoleAggregate> roles = await _mediator.Send(new FindRolesQuery(user.TenantId, roleIds, nameof(payload.Roles)), cancellationToken);
    Dictionary<string, RoleAggregate> rolesById = roles.ToDictionary(x => x.Id.Value, x => x);
    Dictionary<string, RoleAggregate> rolesByUniqueName = roles.ToDictionary(x => x.UniqueName.ToUpper(), x => x);
    foreach (RoleModification modification in payload.Roles)
    {
      string roleId = modification.Role.Trim();
      if (!rolesById.TryGetValue(roleId, out RoleAggregate? role))
      {
        string uniqueName = roleId.ToUpper();
        role = rolesByUniqueName[uniqueName];
      }

      switch (modification.Action)
      {
        case CollectionAction.Add:
          user.AddRole(role);
          break;
        case CollectionAction.Remove:
          user.RemoveRole(role);
          break;
      }
    }

    await _userRepository.SaveAsync(user, cancellationToken);

    return await _userQuerier.ReadAsync(user, cancellationToken);
  }
}
