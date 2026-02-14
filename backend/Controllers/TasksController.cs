using Microsoft.AspNetCore.Mvc;
using backend.Repositories;
using backend;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _repository;

    public TasksController(ITaskRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItems>>> GetTasks()
    {
        return Ok(await _repository.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItems>> GetTask(int id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskItems>> CreateTask(TaskItems task)
    {
        if(string.IsNullOrWhiteSpace(task.Title))
        {
            return BadRequest("Title is required");
        }

        await _repository.AddAsync(task);
        return CreatedAtAction(nameof(GetTasks) , new {id = task.Id}, task);
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> CompleteTask(int id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task == null) return NotFound();

        task.IsCompleted = true;
        await _repository.UpdateAsync(task);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _repository.GetByIdAsync(id);
        if(task == null) return NotFound();

        await _repository.DeleteAsync(id);
        return NoContent();
    }
}

