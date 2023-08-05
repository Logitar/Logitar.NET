using Logitar.Demo.Ui.Domain.Events;
using Logitar.EventSourcing;

namespace Logitar.Demo.Ui.Domain;

public class TodoAggregate : AggregateRoot
{
  private string _text = string.Empty;
  private bool _isDone = false;

  public TodoAggregate(AggregateId id) : base(id)
  {
  }

  public TodoAggregate(string text) : base()
  {
    ApplyChange(new TodoCreatedEvent(text.Trim()));
  }
  protected virtual void Apply(TodoCreatedEvent created) => _text = created.Text;

  public string Text
  {
    get => _text;
    set
    {
      value = value.Trim();
      if (value != _text)
      {
        TodoUpdatedEvent updated = GetLatestEvent<TodoUpdatedEvent>();
        updated.Text = value;
        Apply(updated);
      }
    }
  }
  public bool IsDone
  {
    get => _isDone;
    set
    {
      if (value != _isDone)
      {
        TodoUpdatedEvent updated = GetLatestEvent<TodoUpdatedEvent>();
        updated.IsDone = value;
        Apply(updated);
      }
    }
  }

  public void Delete() => ApplyChange(new TodoDeletedEvent());

  protected virtual void Apply(TodoUpdatedEvent updated)
  {
    if (updated.Text != null)
    {
      _text = updated.Text;
    }
    if (updated.IsDone.HasValue)
    {
      _isDone = updated.IsDone.Value;
    }
  }
  private T GetLatestEvent<T>() where T : DomainEvent, new()
  {
    T? change = Changes.LastOrDefault(change => change is T) as T;
    if (change == null)
    {
      change = new();
      ApplyChange(change);
    }

    return change;
  }

  public override string ToString() => $"{Text} | {base.ToString()}";
}
