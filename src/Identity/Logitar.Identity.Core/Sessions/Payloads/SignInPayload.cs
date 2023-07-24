﻿namespace Logitar.Identity.Core.Sessions.Payloads;

public record SignInPayload
{
  public string? TenantId { get; set; }

  public string UniqueName { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;

  public bool IsPersistent { get; set; }
}
