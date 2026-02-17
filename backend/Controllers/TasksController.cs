using Microsoft.AspNetCore.Mvc;
using backend.Repositories;
using backend.Models;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskTodo _taskTodo;
    private readonly ILogger<TasksController> _logger;

    public TasksController(ITaskTodo taskRepository, ILogger<TasksController> logger)
    {
        _taskTodo = taskRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
    {
        return Ok(await _taskTodo.GetAllTasksAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetTask(int id)
    {
        var task = await _taskTodo.GetTaskByIdAsync(id);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> AddTaskAsync(TaskItem task)
    {
        if(string.IsNullOrWhiteSpace(task.Title))
        {
            return BadRequest("Title is required");
        }

        await _taskTodo.AddTaskAsync(task);
        return CreatedAtAction(nameof(GetTask) , new {id = task.Id}, task);
    }

  [HttpPut("{id}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkTaskAsComplete(int id)
    {
        try
        {
            var success = await _taskTodo.MarkTaskAsCompleteAsync(id);

            if (!success)
            {
                return NotFound(new { message = $"Task with ID {id} not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking task {TaskId} as complete", id);
            return StatusCode(500, "An error occurred while updating the task");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        bool success = await _taskTodo.DeleteTaskAsync(id);

        if(!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}

