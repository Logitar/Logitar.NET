﻿namespace Logitar.Identity.Domain.Passwords;

public abstract record Password
{
  public const char Separator = ':';

  public abstract string Encode();

  public abstract bool IsMatch(string password);
}
