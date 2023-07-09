namespace Logitar;

[Trait(Traits.Category, Categories.Unit)]
public class CollectionExtensionsTests
{
  [Fact(DisplayName = "AddRange: it should add every item in the specified collection.")]
  public void AddRange_it_should_add_every_item_in_the_specified_collection()
  {
    List<int> numbers = new()
    {
      1, 3, 5, 7, 9
    };
    numbers.AddRange(new[] { 0, 2, 4, 6, 8 });
    for (int i = 0; i < 9; i++)
    {
      Assert.Contains(i, numbers);
    }
  }
}
