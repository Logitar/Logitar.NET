namespace Logitar.Demo.Ui.Models.Todo;

public record ReplaceTodoPayload
{
  public string Text { get; set; } = string.Empty;
  public bool IsDone { get; set; }
}
