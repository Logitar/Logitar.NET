﻿using Bogus;

namespace Logitar.EventSourcing;

[Trait(Traits.Category, Categories.Unit)]
public class AggregateRootTests
{
  private readonly Faker _faker = new();

  private readonly PersonAggregate _person;

  public AggregateRootTests()
  {
    _person = new(_faker.Person.FullName);
  }

  [Fact(DisplayName = "ApplyChange: it applies the change correctly.")]
  public void ApplyChange_it_applies_the_change_correctly()
  {
    string name = _faker.Person.FullName;
    string actorId = Guid.NewGuid().ToString();
    DateTime occurredOn = DateTime.Now.AddYears(-20);

    PersonAggregate person = new(name, actorId, occurredOn);
    Assert.Equal(1, person.Version);
    Assert.False(person.IsDeleted);
    Assert.Equal(name, person.FullName);

    List<DomainEvent>? changes = (List<DomainEvent>?)typeof(AggregateRoot)
      .GetField("_changes", BindingFlags.Instance | BindingFlags.NonPublic)
      ?.GetValue(person);
    Assert.NotNull(changes);

    DomainEvent e = changes.Single();
    Assert.NotEqual(Guid.Empty, e.Id);
    Assert.Equal(person.Id, e.AggregateId);
    Assert.Equal(person.Version, e.Version);
    Assert.Equal(actorId, e.ActorId);
    Assert.Equal(occurredOn, e.OccurredOn);
    Assert.Equal(DeleteAction.None, e.DeleteAction);
  }

  [Fact(DisplayName = "ClearChanges: it should clear uncommitted changes correctly.")]
  public void ClearChanges_it_should_clear_uncommitted_changes_correctly()
  {
    PersonAggregate person = new(AggregateId.NewId());
    person.ClearChanges();

    person.Delete();
    Assert.NotEmpty(person.Changes);

    person.ClearChanges();
    Assert.Empty(person.Changes);
  }

  [Fact(DisplayName = "Ctor: it is constructed correctly.")]
  public void Ctor_it_is_constructed_correctly()
  {
    AggregateId id = AggregateId.NewId();
    PersonAggregate aggregate = new(id);
    Assert.Equal(id, aggregate.Id);
  }

  [Fact(DisplayName = "Ctor: it throws ArgumentException when identifier value is missing.")]
  public void Ctor_it_throws_ArgumentException_when_identifier_value_is_missing()
  {
    AggregateId id = new();
    var exception = Assert.Throws<ArgumentException>(() => new PersonAggregate(id));
    Assert.Equal("id", exception.ParamName);
  }

  [Fact(DisplayName = "Dispatch: method Apply is missing does nothing.")]
  public void Dispatch_method_Apply_is_missing_does_nothing()
  {
    _person.Delete();
  }

  [Fact(DisplayName = "Dispatch: it deletes the aggregate.")]
  public void Dispatch_it_deletes_the_aggregate()
  {
    Assert.False(_person.IsDeleted);

    _person.Delete();
    Assert.True(_person.IsDeleted);
  }

  [Fact(DisplayName = "Dispatch: it throws CannotApplyPastEventException when event version is in the past.")]
  public void Dispatch_it_throws_CannotApplyPastEventException_when_event_version_is_in_the_past()
  {
    PersonCreatedEvent e = new(_person.FullName)
    {
      AggregateId = _person.Id,
      Version = 0
    };

    var exception = Assert.Throws<CannotApplyPastEventException>(() => _person.Dispatch(e));
    Assert.Equal(_person.ToString(), exception.Aggregate);
    Assert.Equal(_person.Id.ToString(), exception.AggregateId);
    Assert.Equal(_person.Version, exception.AggregateVersion);
    Assert.Equal(e.ToString(), exception.Event);
    Assert.Equal(e.Id, exception.EventId);
    Assert.Equal(e.Version, exception.EventVersion);
  }

  [Fact(DisplayName = "Dispatch: it throws EventAggregateMismatchException when event does not belong to the aggregate.")]
  public void Dispatch_it_throws_EventAggregateMismatchException_when_event_does_not_belong_to_the_aggregate()
  {
    PersonCreatedEvent e = new(_person.FullName)
    {
      AggregateId = new AggregateId(_person.Id.Value[1..])
    };

    var exception = Assert.Throws<EventAggregateMismatchException>(() => _person.Dispatch(e));
    Assert.Equal(_person.Id.ToString(), exception.AggregateId);
    Assert.Equal(e.ToString(), exception.Event);
    Assert.Equal(e.Id, exception.EventId);
    Assert.Equal(e.AggregateId.ToString(), exception.EventAggregateId);
  }

  [Fact(DisplayName = "Dispatch: it undeletes the aggregate.")]
  public void Dispatch_it_undeletes_the_aggregate()
  {
    _person.Delete();
    Assert.True(_person.IsDeleted);

    _person.Undelete();
    Assert.False(_person.IsDeleted);
  }

  [Fact(DisplayName = "Equals: it is equal to the same type and identifier.")]
  public void Equals_it_is_not_equal_to_the_same_type_and_identifier()
  {
    PersonAggregate other = new(_person.Id);
    Assert.True(_person.Equals(other));
  }

  [Fact(DisplayName = "Equals: it is not equal to different identifier.")]
  public void Equals_it_is_not_equal_to_different_identifier()
  {
    PersonAggregate other = new(new AggregateId(_person.Id.Value[1..]));
    Assert.False(_person.Equals(other));
  }

  [Fact(DisplayName = "Equals: it is not equal to not another aggregate type.")]
  public void Equals_it_is_not_equal_to_not_another_aggregate_type()
  {
    CarAggregate other = new();
    Assert.False(_person.Equals(other));
  }

  [Fact(DisplayName = "Equals: it is not equal to not an aggregate.")]
  public void Equals_it_is_not_equal_to_not_an_aggregate()
  {
    Assert.False(_person.Equals(_person.Id));
  }

  [Fact(DisplayName = "Equals: it is not equal to null.")]
  public void Equals_it_is_not_equal_to_null()
  {
    Assert.False(_person.Equals(null));
  }

  [Fact(DisplayName = "GetHashCode: it returns the correct hash code.")]
  public void GetHashCode_it_returns_the_correct_hash_code()
  {
    int hashCode = HashCode.Combine(_person.GetType(), _person.Id);
    Assert.Equal(hashCode, _person.GetHashCode());
  }

  [Fact(DisplayName = "It should track changes correctly.")]
  public void It_should_track_changes_correctly()
  {
    PersonAggregate person = new(AggregateId.NewId());
    Assert.False(person.HasChanges);
    Assert.Empty(person.Changes);

    person.Delete();
    Assert.True(person.HasChanges);
    Assert.NotEmpty(person.Changes);
  }

  [Fact(DisplayName = "LoadFromChanges: it constructs the correct aggregate.")]
  public void LoadFromChanges_it_constructs_the_correct_aggregate()
  {
    AggregateId id = AggregateId.NewId();
    DomainEvent[] events = new DomainEvent[]
    {
      new PersonCreatedEvent(_faker.Person.FullName)
      {
        AggregateId = id,
        Version = 1,
        OccurredOn = DateTime.Now.AddYears(-20)
      },
      new PersonDeletedChangedEvent(DeleteAction.Delete)
      {
        AggregateId = id,
        Version = 2,
        OccurredOn = DateTime.Now
      }
    };

    PersonAggregate person = AggregateRoot.LoadFromChanges<PersonAggregate>(id, events);
    Assert.Equal(id, person.Id);
    Assert.Equal(2, person.Version);
    Assert.True(person.IsDeleted);
    Assert.Equal(_faker.Person.FullName, person.FullName);
  }

  [Fact(DisplayName = "LoadFromChanges: it throws MissingAggregateConstructorException when public identifier constructor is missing.")]
  public void LoadFromChanges_it_throws_MissingAggregateConstructorException_when_public_identifier_constructor_is_missing()
  {
    AggregateId id = AggregateId.NewId();
    List<DomainEvent> changes = new();
    Assert.Throws<MissingAggregateConstructorException<CarAggregate>>(() => AggregateRoot.LoadFromChanges<CarAggregate>(id, changes));
  }

  [Fact(DisplayName = "ToString: it returns the correct string representation.")]
  public void ToString_it_returns_the_correct_string_representation()
  {
    string expected = string.Concat(_person.GetType(), " (", _person.Id, ')');
    Assert.Equal(expected, _person.ToString());
  }
}
