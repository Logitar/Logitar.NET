using FluentValidation;
using FluentValidation.Results;
using Logitar.EventSourcing;
using Logitar.Identity.Core.Roles.Events;
using Logitar.Identity.Core.Settings;

namespace Logitar.Identity.Core.Roles;

[Trait(Traits.Category, Categories.Unit)]
public class RoleAggregateTests
{
  private const string UniqueName = "admin";
  private const string DisplayName = "Administrator";

  private readonly Bogus.Faker _faker = new();

  private readonly UniqueNameSettings _uniqueNameSettings = new()
  {
    AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"
  };

  private readonly RoleAggregate _role;

  public RoleAggregateTests()
  {
    _role = new(_uniqueNameSettings, UniqueName);
  }

  [Fact(DisplayName = "Constructor should throw ValidationException when validation fails.")]
  public void Constructor_should_throw_ValidationException_when_validation_fails()
  {
    var exception = Assert.Throws<ValidationException>(() => new RoleAggregate(_uniqueNameSettings, "Test123!"));
    ValidationFailure failure = exception.Errors.Single();
    Assert.Equal("AllowedCharactersValidator", failure.ErrorCode);
    Assert.Equal("UniqueName", failure.PropertyName);
  }

  [Theory(DisplayName = "It should be constructed correctly with_arguments.")]
  [InlineData(UniqueName)]
  [InlineData(UniqueName, "0adddb66-f1bf-49ba-85ed-b715998d083d")]
  [InlineData(" admin   ", "", "968")]
  public void It_should_be_constructed_correctly_with_arguments(string uniqueName, string? tenantId = null, string? id = null)
  {
    AggregateId? roleId = id == null ? null : new(id);
    RoleAggregate role = new(_uniqueNameSettings, uniqueName, tenantId, roleId);
    Assert.Equal(uniqueName.Trim(), role.UniqueName);
    Assert.Equal(tenantId?.CleanTrim(), role.TenantId);

    if (id == null)
    {
      Assert.NotEqual(default, role.Id);
    }
    else
    {
      Assert.Equal(roleId, role.Id);
    }
  }

  [Theory(DisplayName = "It should be constructed correctly with identifier.")]
  [InlineData("123")]
  public void It_should_be_constructed_correctly_with_identifier(string id)
  {
    AggregateId roleId = new(id);
    RoleAggregate role = new(roleId);
    Assert.Equal(roleId, role.Id);
  }

  [Fact(DisplayName = "It should be deleted correctly.")]
  public void It_should_be_deleted_correctly()
  {
    Assert.False(_role.IsDeleted);

    _role.Delete();
    Assert.True(_role.IsDeleted);
  }

  [Fact(DisplayName = "It should not remove custom attribute if it is not found.")]
  public void It_should_not_remove_custom_attribute_if_it_is_not_found()
  {
    _role.SetCustomAttribute("read_users", "true");

    _role.RemoveCustomAttribute("write_users");
    Assert.Contains(_role.CustomAttributes.Keys, key => key == "read_users");
    Assert.DoesNotContain(_role.CustomAttributes.Keys, key => key == "write_users");

    RoleModifiedEvent e = (RoleModifiedEvent)_role.Changes.Last(e => e is RoleModifiedEvent);
    Assert.DoesNotContain(e.CustomAttributes.Keys, key => key == "write_users");
  }
  [Fact(DisplayName = "It should remove custom attribute if it is found.")]
  public void It_should_remove_custom_attribute_if_it_is_found()
  {
    _role.SetCustomAttribute("read_users", "true");
    _role.SetCustomAttribute("write_users", "true");

    _role.RemoveCustomAttribute("   write_users ");
    Assert.Contains(_role.CustomAttributes.Keys, key => key == "read_users");
    Assert.DoesNotContain(_role.CustomAttributes.Keys, key => key == "write_users");

    RoleModifiedEvent e = (RoleModifiedEvent)_role.Changes.Last(e => e is RoleModifiedEvent);
    Assert.Null(e.CustomAttributes["write_users"]);
  }

  [Theory(DisplayName = "It should return the correct string representation.")]
  [InlineData(UniqueName)]
  [InlineData(UniqueName, DisplayName)]
  public void It_should_return_the_correct_string_representation(string uniqueName, string? displayName = null)
  {
    _role.DisplayName = displayName;
    string expected = string.Format("{0} | {1} ({2})", displayName ?? uniqueName, typeof(RoleAggregate), _role.Id);
    Assert.Equal(expected, _role.ToString());
  }

  [Theory(DisplayName = "It should set the correct custom attribute.")]
  [InlineData(" ProfileId ", "   76   ")]
  public void It_should_set_the_correct_custom_attribute(string key, string value)
  {
    _role.SetCustomAttribute(key, value);
    Assert.Equal(value.Trim(), _role.CustomAttributes[key.Trim()]);

    RoleModifiedEvent e = (RoleModifiedEvent)_role.Changes.Last(e => e is RoleModifiedEvent);
    Assert.Equal(value.Trim(), e.CustomAttributes[key.Trim()]);
  }
  [Fact(DisplayName = "It should set the correct custom attribute only if it is modified.")]
  public void It_should_set_the_correct_custom_attribute_only_if_it_is_modified()
  {
    _role.SetCustomAttribute("ProfileId", "51");
    RoleModifiedEvent e = (RoleModifiedEvent)_role.Changes.Last(e => e is RoleModifiedEvent);
    Assert.Equal("51", e.CustomAttributes["ProfileId"]);

    _role.ClearChanges();
    _role.SetCustomAttribute("ProfileId", "51");
    Assert.DoesNotContain(_role.Changes, e => e is RoleModifiedEvent);
  }

  [Theory(DisplayName = "It should set the correct Description.")]
  [InlineData(null)]
  [InlineData("  This is a test role.  ")]
  public void It_should_set_the_correct_Description(string? description)
  {
    _role.Description = description;
    Assert.Equal(description?.CleanTrim(), _role.Description);
  }
  [Fact(DisplayName = "It should set the correct Description only if it is modified.")]
  public void It_should_set_the_correct_Description_only_if_it_is_modified()
  {
    _role.Description = "This is a test role.";
    RoleModifiedEvent e = (RoleModifiedEvent)_role.Changes.Last(e => e is RoleModifiedEvent);
    Assert.True(e.Description.IsModified);

    _role.ClearChanges();
    _role.Description = "This is a test role.";
    Assert.DoesNotContain(_role.Changes, e => e is RoleModifiedEvent);
  }

  [Theory(DisplayName = "It should set the correct DisplayName.")]
  [InlineData(null)]
  [InlineData(DisplayName)]
  [InlineData("  Guest  ")]
  public void It_should_set_the_correct_DisplayName(string? displayName)
  {
    _role.DisplayName = displayName;
    Assert.Equal(displayName?.CleanTrim(), _role.DisplayName);
  }
  [Fact(DisplayName = "It should set the correct DisplayName only if it is modified.")]
  public void It_should_set_the_correct_DisplayName_only_if_it_is_modified()
  {
    _role.DisplayName = DisplayName;
    RoleModifiedEvent e = (RoleModifiedEvent)_role.Changes.Last(e => e is RoleModifiedEvent);
    Assert.True(e.DisplayName.IsModified);

    _role.ClearChanges();
    _role.DisplayName = DisplayName;
    Assert.DoesNotContain(_role.Changes, e => e is RoleModifiedEvent);
  }

  [Theory(DisplayName = "It should set the correct UniqueName.")]
  [InlineData(UniqueName)]
  [InlineData("  admin  ")]
  public void It_should_set_the_correct_UniqueName(string uniqueName)
  {
    _role.SetUniqueName(_uniqueNameSettings, uniqueName);
    Assert.Equal(uniqueName.Trim(), _role.UniqueName);
  }
  [Fact(DisplayName = "It should set the correct UniqueName only if it is modified.")]
  public void It_should_set_the_correct_UniqueName_only_if_it_is_modified()
  {
    _role.SetUniqueName(_uniqueNameSettings, "guest");
    Assert.Contains(_role.Changes, e => e is RoleUniqueNameChangedEvent);

    _role.ClearChanges();
    _role.SetUniqueName(_uniqueNameSettings, "guest");
    Assert.DoesNotContain(_role.Changes, e => e is RoleUniqueNameChangedEvent);
  }

  [Fact(DisplayName = "It should throw ValidationException when custom attribute key is not valid.")]
  public void It_should_throw_ValidationException_when_custom_attribute_key_is_not_valid()
  {
    var exception = Assert.Throws<ValidationException>(() => _role.SetCustomAttribute(string.Empty, "true"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));
  }
  [Fact(DisplayName = "It should throw ValidationException when custom attribute value is not valid.")]
  public void It_should_throw_ValidationException_when_custom_attribute_value_is_not_valid()
  {
    var exception = Assert.Throws<ValidationException>(() => _role.SetCustomAttribute("write_users", string.Empty));
    ValidationFailure failure = exception.Errors.Single();
    Assert.Equal("Value", failure.PropertyName);
  }

  [Fact(DisplayName = "It should throw ValidationException when display name is not valid.")]
  public void It_should_throw_ValidationException_when_display_name_is_not_valid()
  {
    string value = _faker.Random.String(length: 300, minChar: 'a', maxChar: 'z');
    var exception = Assert.Throws<ValidationException>(() => _role.DisplayName = value);
    ValidationFailure failure = exception.Errors.Single();
    Assert.Equal("MaximumLengthValidator", failure.ErrorCode);
    Assert.Equal("DisplayName", failure.PropertyName);
  }

  [Fact(DisplayName = "It should throw ValidationException when unique name is not valid.")]
  public void It_should_throw_ValidationException_when_unique_name_is_not_valid()
  {
    var exception = Assert.Throws<ValidationException>(() => _role.SetUniqueName(_uniqueNameSettings, "admin!"));
    ValidationFailure failure = exception.Errors.Single();
    Assert.Equal("AllowedCharactersValidator", failure.ErrorCode);
    Assert.Equal("UniqueName", failure.PropertyName);
  }
}
