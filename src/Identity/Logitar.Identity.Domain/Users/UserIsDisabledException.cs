﻿namespace Logitar.Identity.Domain.Users;

public class UserIsDisabledException : Exception
{
  public UserIsDisabledException(UserAggregate user) : base($"The user 'Id={user.Id}' is disabled.")
  {
    User = user.ToString();
  }

  public string User
  {
    get => (string)Data[nameof(User)]!;
    private set => Data[nameof(User)] = value;
  }
}
