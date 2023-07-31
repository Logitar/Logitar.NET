using AutoMapper;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Mappings;

public class SessionProfile : Profile
{
  public SessionProfile()
  {
    CreateMap<SessionEntity, Session>()
      .IncludeBase<AggregateEntity, Aggregate>()
      .ForMember(x => x.SignedOutBy, x => x.MapFrom(y => ActorEntity.Deserialize(y.SignedOutById, y.SignedOutBy)))
      .ForMember(x => x.SignedOutOn, x => x.MapFrom(y => DateTimeHelper.ToUniversalTime(y.SignedOutOn)))
      .ForMember(x => x.CustomAttributes, x => x.MapFrom(y => y.GetCustomAttributes()));
  }
}
