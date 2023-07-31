﻿using Logitar.Identity.Core.Payloads;
using Logitar.Identity.Domain;

namespace Logitar.Identity.Core.Roles.Payloads;

public record UpdateRolePayload
{
  public string? UniqueName { get; set; }
  public MayBe<string>? DisplayName { get; set; }
  public MayBe<string>? Description { get; set; }

  public IEnumerable<CustomAttributeModification> CustomAttributes { get; set; } = Enumerable.Empty<CustomAttributeModification>();
}
