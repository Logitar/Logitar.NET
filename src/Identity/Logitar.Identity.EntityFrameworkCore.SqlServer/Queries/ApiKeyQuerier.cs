using AutoMapper;
using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Queries;

public class ApiKeyQuerier : IApiKeyQuerier
{
  private readonly DbSet<ApiKeyEntity> _apiKeys;
  private readonly IMapper _mapper;

  public ApiKeyQuerier(IdentityContext context, IMapper mapper)
  {
    _apiKeys = context.ApiKeys;
    _mapper = mapper;
  }

  public async Task<ApiKey> ReadAsync(ApiKeyAggregate apiKey, CancellationToken cancellationToken)
  {
    return await ReadAsync(apiKey.Id.Value, cancellationToken)
      ?? throw new EntityNotFoundException<ApiKeyEntity>(apiKey.Id.Value);
  }
  public async Task<ApiKey?> ReadAsync(string id, CancellationToken cancellationToken)
  {
    ApiKeyEntity? apiKey = await _apiKeys.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == id, cancellationToken);

    return _mapper.Map<ApiKey?>(apiKey);
  }
}
