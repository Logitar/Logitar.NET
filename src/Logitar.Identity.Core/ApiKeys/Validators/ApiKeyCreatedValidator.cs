﻿using FluentValidation;
using Logitar.Identity.Core.ApiKeys.Events;

namespace Logitar.Identity.Core.ApiKeys.Validators;

/// <summary>
/// The validator used to validate instances of <see cref="ApiKeyCreatedEvent"/>.
/// </summary>
public class ApiKeyCreatedValidator : AbstractValidator<ApiKeyCreatedEvent>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyCreatedValidator"/> class.
  /// </summary>
  /// <param name="titleValidator">The validator used to validate titles.</param>
  public ApiKeyCreatedValidator(IValidator<string> titleValidator)
  {
    RuleFor(x => x.TenantId).NullOrNotEmpty()
      .MaximumLength(byte.MaxValue);

    RuleFor(x => x.Secret).NotNull();

    RuleFor(x => x.Title).SetValidator(titleValidator);
  }
}