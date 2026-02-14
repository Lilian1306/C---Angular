using Microsoft.AspNetCore.Mvc;
using backend.Repositories;
using backend.Models;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskTodo _repository;

    public TasksController(ITaskTodo repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
    {
        return Ok(await _repository.GetAllTasksAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetTask(int id)
    {
        var task = await _repository.GetTaskByIdAsync(id);
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

        await _repository.AddTaskAsync(task);
        return CreatedAtAction(nameof(GetTasks) , new {id = task.Id}, task);
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> CompleteTask(int id)
    {
        bool wasUpdated = await _repository.UpdateTaskCompletionAsync(id);

        if(!wasUpdated)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        bool wasUpdated = await _repository.DeleteTaskAsync(id);

        if(!wasUpdated)
        {
            return NotFound();
        }

        return NoContent();
    }
}

