using AutoMapper;
using Logitar.Identity.Core.Models;
using Logitar.Identity.EntityFrameworkCore.Relational.Actors;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Mappings;

public class AggregateProfile : Profile
{
  public AggregateProfile()
  {
    CreateMap<AggregateEntity, Aggregate>()
      .ForMember(x => x.Id, x => x.MapFrom(y => y.AggregateId))
      .ForMember(x => x.CreatedBy, x => x.MapFrom(y => ActorEntity.Deserialize(y.CreatedById, y.CreatedBy)))
      .ForMember(x => x.CreatedOn, x => x.MapFrom(y => DateTimeHelper.ToUniversalTime(y.CreatedOn)))
      .ForMember(x => x.UpdatedBy, x => x.MapFrom(y => ActorEntity.Deserialize(y.UpdatedById, y.UpdatedBy)))
      .ForMember(x => x.UpdatedOn, x => x.MapFrom(y => DateTimeHelper.ToUniversalTime(y.UpdatedOn)));
  }
}
