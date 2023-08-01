using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using MediatR;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Core.Users.Queries;

public class ReadUserQueryHandler : IRequestHandler<ReadUserQuery, User?>
{
  private readonly IUserQuerier _userQuerier;
  private readonly IOptions<UserSettings> _userSettings;

  public ReadUserQueryHandler(IUserQuerier userQuerier, IOptions<UserSettings> userSettings)
  {
    _userQuerier = userQuerier;
    _userSettings = userSettings;
  }

  public async Task<User?> Handle(ReadUserQuery query, CancellationToken cancellationToken)
  {
    Dictionary<string, User> users = new(capacity: 3);

    if (query.Id != null)
    {
      User? user = await _userQuerier.ReadAsync(query.Id, cancellationToken);
      if (user != null)
      {
        users[user.Id] = user;
      }
    }

    if (query.UniqueName != null)
    {
      User? user = await _userQuerier.ReadAsync(query.TenantId, query.UniqueName, cancellationToken);
      if (user != null)
      {
        users[user.Id] = user;
      }
      else
      {
        UserSettings userSettings = _userSettings.Value;
        if (userSettings.RequireUniqueEmail)
        {
          EmailAddress email = new(query.UniqueName);
          IEnumerable<User> foundUsers = await _userQuerier.ReadAsync(query.TenantId, email, cancellationToken);
          if (foundUsers.Count() == 1)
          {
            user = foundUsers.Single();
            users[user.Id] = user;
          }
        }
      }
    }

    if (query.ExternalIdentifierKey != null && query.ExternalIdentifierValue != null)
    {
      User? user = await _userQuerier.ReadAsync(query.TenantId, query.ExternalIdentifierKey,
        query.ExternalIdentifierValue, cancellationToken);
      if (user != null)
      {
        users[user.Id] = user;
      }
    }

    if (users.Count > 1)
    {
      throw new TooManyResultsException<User>(expected: 1, actual: users.Count);
    }

    return users.Values.SingleOrDefault();
  }
}
