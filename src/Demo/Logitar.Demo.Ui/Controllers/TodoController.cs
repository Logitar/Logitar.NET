using Logitar.Demo.Ui.Domain;
using Logitar.Demo.Ui.Models;
using Logitar.Demo.Ui.Models.Todo;
using Logitar.EventSourcing;
using Microsoft.AspNetCore.Mvc;

namespace Logitar.Demo.Ui.Controllers;

[ApiController]
[Route("todos")]
public class TodoController : ControllerBase
{
  private readonly IAggregateRepository _aggregateRepository;

  public TodoController(IAggregateRepository aggregateRepository)
  {
    _aggregateRepository = aggregateRepository;
  }

  [HttpPost]
  public async Task<ActionResult<Todo>> CreateAsync([FromBody] CreateTodoPayload payload, CancellationToken cancellationToken)
  {
    TodoAggregate todo = new(payload.Text);
    await _aggregateRepository.SaveAsync(todo, cancellationToken);

    Todo result = new(todo);
    Uri uri = new($"{Request.Scheme}://{Request.Host}/todos/{result.Id}");

    return Created(uri, result);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<Todo>> DeleteAsync(string id, CancellationToken cancellationToken)
  {
    TodoAggregate? todo = await _aggregateRepository.LoadAsync<TodoAggregate>(new AggregateId(id), cancellationToken);
    if (todo == null)
    {
      return NotFound();
    }

    todo.Delete();

    await _aggregateRepository.SaveAsync(todo, cancellationToken);

    return Ok(new Todo(todo));
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Todo>> ReadAsync(string id, CancellationToken cancellationToken)
  {
    TodoAggregate? todo = await _aggregateRepository.LoadAsync<TodoAggregate>(new AggregateId(id), cancellationToken);
    if (todo == null)
    {
      return NotFound();
    }

    return Ok(new Todo(todo));
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<Todo>>> SearchAsync(string? search, bool? isDone,
    string? sort, bool isDescending, int skip, int limit, CancellationToken cancellationToken)
  {
    IEnumerable<TodoAggregate> todos = await _aggregateRepository.LoadAsync<TodoAggregate>(cancellationToken);
    if (search != null)
    {
      todos = todos.Where(x => x.Text.ToLower().Contains(search.ToLower()));
    }
    if (isDone.HasValue)
    {
      todos = todos.Where(x => x.IsDone == isDone.Value);
    }

    long total = todos.LongCount();

    if (sort != null)
    {
      switch (sort)
      {
        case "Text":
          todos = isDescending ? todos.OrderByDescending(x => x.Text) : todos.OrderBy(x => x.Text);
          break;
        case "UpdatedOn":
          todos = isDescending ? todos.OrderByDescending(x => x.UpdatedOn) : todos.OrderBy(x => x.UpdatedOn);
          break;
      }
    }

    if (skip > 0)
    {
      todos = todos.Skip(skip);
    }
    if (limit > 0)
    {
      todos = todos.Take(limit);
    }

    return Ok(new SearchResults<Todo>(todos.Select(todo => new Todo(todo)), total));
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<Todo>> ReplaceAsync(string id, [FromBody] ReplaceTodoPayload payload, CancellationToken cancellationToken)
  {
    TodoAggregate? todo = await _aggregateRepository.LoadAsync<TodoAggregate>(new AggregateId(id), cancellationToken);
    if (todo == null)
    {
      return NotFound();
    }

    todo.Text = payload.Text;
    todo.IsDone = payload.IsDone;

    await _aggregateRepository.SaveAsync(todo, cancellationToken);

    return Ok(new Todo(todo));
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult<Todo>> UpdateAsync(string id, [FromBody] UpdateTodoPayload payload, CancellationToken cancellationToken)
  {
    TodoAggregate? todo = await _aggregateRepository.LoadAsync<TodoAggregate>(new AggregateId(id), cancellationToken);
    if (todo == null)
    {
      return NotFound();
    }

    if (payload.Text != null)
    {
      todo.Text = payload.Text;
    }
    if (payload.IsDone.HasValue)
    {
      todo.IsDone = payload.IsDone.Value;
    }

    await _aggregateRepository.SaveAsync(todo, cancellationToken);

    return Ok(new Todo(todo));
  }
}
