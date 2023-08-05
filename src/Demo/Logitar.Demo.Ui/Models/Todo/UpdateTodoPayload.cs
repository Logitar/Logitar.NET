namespace Logitar.Demo.Ui.Models.Todo;

public record UpdateTodoPayload
{
  public string? Text { get; set; }
  public bool? IsDone { get; set; }
}
