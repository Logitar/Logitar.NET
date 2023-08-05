namespace Logitar.Demo.Ui.Models.Todo;

public record CreateTodoPayload
{
  public string Text { get; set; } = string.Empty;
}
