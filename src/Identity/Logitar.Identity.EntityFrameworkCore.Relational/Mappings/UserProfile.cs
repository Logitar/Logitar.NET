using AutoMapper;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.EntityFrameworkCore.Relational.Actors;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Mappings;

public class UserProfile : Profile
{
  public UserProfile()
  {
    CreateMap<UserEntity, User>()
      .IncludeBase<AggregateEntity, Aggregate>()
      .ForMember(x => x.PasswordChangedBy, x => x.MapFrom(y => ActorEntity.Deserialize(y.PasswordChangedById, y.PasswordChangedBy)))
      .ForMember(x => x.DisabledBy, x => x.MapFrom(y => ActorEntity.Deserialize(y.DisabledById, y.DisabledBy)))
      .ForMember(x => x.Address, x => x.MapFrom(GetAddress))
      .ForMember(x => x.Email, x => x.MapFrom(GetEmail))
      .ForMember(x => x.Phone, x => x.MapFrom(GetPhone))
      .ForMember(x => x.Birthdate, x => x.MapFrom(y => DateTimeHelper.ToUniversalTime(y.Birthdate)))
      .ForMember(x => x.CustomAttributes, x => x.MapFrom(y => y.GetCustomAttributes()));
  }

  private static Address? GetAddress(UserEntity user, User _)
  {
    if (user.AddressStreet == null || user.AddressLocality == null
      || user.AddressCountry == null || user.AddressFormatted == null)
    {
      return null;
    }

    return new Address
    {
      Street = user.AddressStreet,
      Locality = user.AddressLocality,
      Country = user.AddressCountry,
      Region = user.AddressRegion,
      PostalCode = user.AddressPostalCode,
      Formatted = user.AddressFormatted,
      IsVerified = user.IsAddressVerified,
      VerifiedBy = ActorEntity.Deserialize(user.AddressVerifiedById, user.AddressVerifiedBy),
      VerifiedOn = DateTimeHelper.ToUniversalTime(user.AddressVerifiedOn)
    };
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
      VerifiedOn = DateTimeHelper.ToUniversalTime(user.EmailVerifiedOn)
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
      VerifiedOn = DateTimeHelper.ToUniversalTime(user.PhoneVerifiedOn)
    };
  }
}
