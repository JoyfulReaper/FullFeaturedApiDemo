using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoLibrary;
using TodoLibrary.DataAccess;

namespace TodoApi.Controllers.v2;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2.0")]
[ApiController]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class TodoController : ControllerBase
{
    private readonly ITodoData _todoData;
    private readonly ILogger<TodoController> _logger;

    public TodoController(ITodoData todoData,
        ILogger<TodoController> logger)
	{
        _todoData = todoData;
        _logger = logger;
    }

    private string GetUserId()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
        {
            throw new Exception("Unable to get the Id of the currently logged in user.");
        }
        return userId;
    }

    /// <summary>
    /// Get all todos for the logged in user
    /// </summary>
    /// <returns>All todos for the logged in user</returns>
    [HttpGet(Name = "GetAllTodos")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Todo))]

    public async Task<ActionResult<List<Todo>>> Get()
    {
        _logger.LogInformation("GET: api/Todos");

        try
        {
            var output = await _todoData.GetAll(GetUserId());
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/Todos failed.");
            return BadRequest();
        }
    }

    [HttpGet("{todoId}", Name = "GetTodo")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Todo))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Todo>> Get(int todoId)
    {
        _logger.LogInformation("GET: api/Todos/{TodoId}", todoId);

        try
        {
            var output = await _todoData.Get(GetUserId(), todoId);
            if(output == null)
            {
                return NotFound();
            }

            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to {ApiPath} failed. The Id was {TodoId}",
                $"api/Todos/Id",
                todoId);
            return BadRequest();
        }
    }

    [HttpPost(Name = "CreateTodo")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Todo))]
    public async Task<ActionResult<Todo>> Post([FromBody] string task)
    {
        _logger.LogInformation("POST: api/Todos (Task: {Task}", task);

        try
        {
            var output = await _todoData.Create(GetUserId(), task);
            if(output is null)
            {
                throw new Exception("Failed retrieve saved Todo item.");
            }
            return CreatedAtAction(nameof(Get), new {todoId = output.TodoId}, output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The POST call to api/Todos failed. Task value was {Task}", task);
            return BadRequest();
        }
    }

    [HttpPut("{todoId}", Name = "UpdateTodoTask")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Put(int todoId, [FromBody] string task)
    {
        _logger.LogInformation("PUT: api/Todos/{TodoId} (Task: {Task}", todoId, task);

        try
        {
            await _todoData.UpdateTask(GetUserId(), todoId, task);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The PUT call to api/Todos/{TodoId} failed. Task value was {Task}", todoId, task);
            return BadRequest();
        }
    }

    [HttpPut("{todoId}/Complete", Name = "CompleteTodo")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Complete(int todoId)
    {
        _logger.LogInformation("PUT: api/Todos/{TodoId}/Complete ", todoId);

        try
        {
            await _todoData.Complete(GetUserId(), todoId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The PUT call to api/Todos/{TodoId}/Complete failed.", todoId);
            return BadRequest();
        }
    }

    [HttpDelete("{todoId}", Name = "DeleteTodo")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int todoId)
    {
        _logger.LogInformation("DELETE: api/Todos/{TodoId} ", todoId);

        try
        {
            await _todoData.Delete(GetUserId(), todoId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The DELETE call to api/Todos/{TodoId} failed.", todoId);
            return BadRequest();
        }
    }
}
