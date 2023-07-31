using AutoMapper;
using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.Models;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Mappings;

public class ApiKeyProfile : Profile
{
  public ApiKeyProfile()
  {
    CreateMap<ApiKeyEntity, ApiKey>()
      .IncludeBase<AggregateEntity, Aggregate>()
      .ForMember(x => x.ExpiresOn, x => x.MapFrom(y => DateTimeHelper.ToUniversalTime(y.ExpiresOn)))
      .ForMember(x => x.Secret, x => x.Ignore())
      .ForMember(x => x.CustomAttributes, x => x.MapFrom(y => y.GetCustomAttributes()));
  }
}
