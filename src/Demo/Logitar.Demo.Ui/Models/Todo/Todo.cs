using Logitar.Demo.Ui.Domain;

namespace Logitar.Demo.Ui.Models.Todo;

public record Todo
{
  public Todo()
  {
  }
  public Todo(TodoAggregate todo) : this()
  {
    Id = todo.Id.Value;
    CreatedBy = todo.CreatedBy.Value;
    CreatedOn = todo.CreatedOn;
    UpdatedBy = todo.UpdatedBy.Value;
    UpdatedOn = todo.UpdatedOn;
    Version = todo.Version;

    Text = todo.Text;
    IsDone = todo.IsDone;
  }

  public string Id { get; set; } = string.Empty;
  public string CreatedBy { get; set; } = string.Empty;
  public DateTime CreatedOn { get; set; }
  public string UpdatedBy { get; set; } = string.Empty;
  public DateTime UpdatedOn { get; set; }
  public long Version { get; set; }

  public string Text { get; set; } = string.Empty;
  public bool IsDone { get; set; }
}
