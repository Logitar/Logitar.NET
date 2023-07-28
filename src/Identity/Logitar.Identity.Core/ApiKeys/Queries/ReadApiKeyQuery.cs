using Logitar.Identity.Core.ApiKeys.Models;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Queries;

public record ReadApiKeyQuery : IRequest<ApiKey?>
{
  public ReadApiKeyQuery(string? id = null)
  {
    Id = id;
  }

  public string? Id { get; }
}
