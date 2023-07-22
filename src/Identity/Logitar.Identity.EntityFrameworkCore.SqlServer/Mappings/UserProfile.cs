using AutoMapper;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Mappings;

public class UserProfile : Profile
{
  public UserProfile()
  {
    CreateMap<UserEntity, User>()
      .IncludeBase<AggregateEntity, Aggregate>()
      .ForMember(x => x.PasswordChangedBy, x => x.MapFrom(y => ActorEntity.Deserialize(y.PasswordChangedById, y.PasswordChangedBy)))
      .ForMember(x => x.DisabledBy, x => x.MapFrom(y => ActorEntity.Deserialize(y.DisabledById, y.DisabledBy)))
      .ForMember(x => x.Email, x => x.MapFrom(GetEmail));
  }

  private static Email? GetEmail(UserEntity user, User _)
  {
    if (user.EmailAddress == null)
    {
      return null;
    }

    return new Email
    {
      Address = user.EmailAddress,
      IsVerified = user.IsEmailVerified,
      VerifiedBy = ActorEntity.Deserialize(user.EmailVerifiedById, user.EmailVerifiedBy),
      VerifiedOn = user.EmailVerifiedOn
    };
  }
}
