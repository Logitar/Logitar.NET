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
      .ForMember(x => x.SignedOutBy, x => x.MapFrom(y => ActorEntity.Deserialize(y.SignedOutById, y.SignedOutBy)));
  }
}
