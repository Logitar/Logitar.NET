using AutoMapper;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Roles.Models;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Mappings;

public class RoleProfile : Profile
{
  public RoleProfile()
  {
    CreateMap<RoleEntity, Role>()
      .IncludeBase<AggregateEntity, Aggregate>()
      .ForMember(x => x.CustomAttributes, x => x.MapFrom(y => y.GetCustomAttributes()));
  }
}
