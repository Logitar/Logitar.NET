using Bogus;

namespace Logitar.Data;

[Trait(Traits.Category, Categories.Unit)]
public class JoinTests
{
  private static readonly Faker _faker = new();

  [Fact(DisplayName = "Ctor: it constructs the correct Join.")]
  public void Ctor_it_constructs_the_correct_Join()
  {
    TableId roles = new("Roles");

    JoinKind kind = _faker.PickRandom(Enum.GetValues<JoinKind>());
    ColumnId right = new("UserId", new TableId("Users"));
    ColumnId left = new("RoleId", roles);
    OperatorCondition condition = new(new ColumnId("IsDeleted", roles), Operators.IsEqualTo(false));

    Join join = new(kind, right, left, condition);
    Assert.Equal(kind, join.Kind);
    Assert.Same(right, join.Right);
    Assert.Same(left, join.Left);
    Assert.Same(condition, join.Condition);
  }

  [Fact(DisplayName = "Ctor: it constructs the correct Join using default JoinKind.")]
  public void Ctor_it_constructs_the_correct_Join_using_default_JoinKind()
  {
    ColumnId right = new("UserId", new TableId("Users"));
    ColumnId left = new("RoleId", new TableId("Roles"));
    Join join = new(right, left);
    Assert.Equal(default, join.Kind);
  }

  [Fact(DisplayName = "Ctor: it throws ArgumentException when left name is null.")]
  public void Ctor_it_throws_ArgumentException_when_left_name_is_null()
  {
    ColumnId right = new("UserId", new TableId("Users"));
    ColumnId left = ColumnId.All(new TableId("Roles"));
    var exception = Assert.Throws<ArgumentException>(() => new Join(right, left));
    Assert.Equal("left", exception.ParamName);
    Assert.StartsWith("The column name is required.", exception.Message);
  }

  [Fact(DisplayName = "Ctor: it throws ArgumentException when left table is null.")]
  public void Ctor_it_throws_ArgumentException_when_left_table_is_null()
  {
    ColumnId right = new("UserId", new TableId("Users"));
    ColumnId left = new("RoleId");
    var exception = Assert.Throws<ArgumentException>(() => new Join(right, left));
    Assert.Equal("left", exception.ParamName);
    Assert.StartsWith("The column table is required.", exception.Message);
  }

  [Fact(DisplayName = "Ctor: it throws ArgumentException when right name is null.")]
  public void Ctor_it_throws_ArgumentException_when_right_name_is_null()
  {
    ColumnId right = ColumnId.All(new TableId("Users"));
    ColumnId left = new("RoleId", new TableId("Roles"));
    var exception = Assert.Throws<ArgumentException>(() => new Join(right, left));
    Assert.Equal("right", exception.ParamName);
    Assert.StartsWith("The column name is required.", exception.Message);
  }

  [Fact(DisplayName = "Ctor: it throws ArgumentException when right table is null.")]
  public void Ctor_it_throws_ArgumentException_when_right_table_is_null()
  {
    ColumnId right = new("UserId");
    ColumnId left = new("RoleId", new TableId("Roles"));
    var exception = Assert.Throws<ArgumentException>(() => new Join(right, left));
    Assert.Equal("right", exception.ParamName);
    Assert.StartsWith("The column table is required.", exception.Message);
  }
}
