﻿using AutoMapper;
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
      .ForMember(x => x.Email, x => x.MapFrom(GetEmail))
      .ForMember(x => x.Phone, x => x.MapFrom(GetPhone))
      .ForMember(x => x.Birthdate, x => x.MapFrom(y => MappingHelper.ToUniversalTime(y.Birthdate)));
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

  private static Phone? GetPhone(UserEntity user, User _)
  {
    if (user.PhoneNumber == null || user.PhoneE164Formatted == null)
    {
      return null;
    }

    return new Phone
    {
      CountryCode = user.PhoneCountryCode,
      Number = user.PhoneNumber,
      Extension = user.PhoneExtension,
      E164Formatted = user.PhoneE164Formatted,
      IsVerified = user.IsPhoneVerified,
      VerifiedBy = ActorEntity.Deserialize(user.PhoneVerifiedById, user.PhoneVerifiedBy),
      VerifiedOn = user.PhoneVerifiedOn
    };
  }
}
